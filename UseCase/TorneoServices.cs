using Aplicación.Exceptions;
using Aplicación.Interfaces;
using Dominio.Entidades;
using static Dominio.Entidades.EnumeracionesEstados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicación.UseCase
{
    public class TorneoServices : ITorneoServices
    {
        private readonly ITorneoCommand _torneoCommand;
        private readonly ITorneoQuery _torneoQuery;
        private readonly ICanchaQuery _canchaQuery;

        public TorneoServices(ITorneoCommand torneoCommand, ITorneoQuery torneoQuery, ICanchaQuery canchaQuery)
        {
            _torneoCommand = torneoCommand;
            _torneoQuery = torneoQuery;
            _canchaQuery = canchaQuery;
        }

        public async Task CreateTorneo(Torneo torneo)
        {
            if (torneo.FechaInicio < DateTime.Today)
            {
                throw new ArgumentException("La fecha de inicio no puede ser anterior a la fecha actual.");
            }

            if (torneo.FechaFin < torneo.FechaInicio)
            {
                throw new ArgumentException("La fecha de finalización debe ser posterior a la fecha de inicio.");
            }

            if (torneo.CantidadMinimaParticipantes <= 0 || torneo.CantidadMaximaParticipantes < torneo.CantidadMinimaParticipantes)
            {
                throw new ArgumentException("Los cupos de participantes mínimo y máximo son inválidos.");
            }

            torneo.Estado = EstadoTorneo.Planificado;
            await _torneoCommand.InsertTorneo(torneo);
        }

        public async Task UpdateTorneo(Torneo torneo)
        {
            var existente = await _torneoQuery.GetTorneoById(torneo.Id);
            if (existente == null)
            {
                throw new KeyNotFoundException("El torneo no existe.");
            }

            // RF-40: No se permite modificar modalidad ni fecha de inicio si ya tiene inscriptos o comenzó
            if (existente.Inscripciones.Count > 0 || existente.Estado != EstadoTorneo.Planificado)
            {
                if (existente.Modalidad != torneo.Modalidad || existente.FechaInicio != torneo.FechaInicio)
                {
                    throw new InvalidOperationException("No se puede modificar la modalidad ni la fecha de inicio cuando el torneo ya posee inscriptos o ha comenzado.");
                }
            }

            await _torneoCommand.UpdateTorneo(torneo);
        }

        public async Task CancelarTorneo(Guid id)
        {
            var torneo = await _torneoQuery.GetTorneoById(id);
            if (torneo == null)
            {
                throw new KeyNotFoundException("El torneo no existe.");
            }

            await _torneoCommand.CancelTorneo(id);
        }

        public async Task DeleteTorneo(Guid id)
        {
            var existente = await _torneoQuery.GetTorneoById(id);
            if (existente == null)
            {
                throw new KeyNotFoundException($"No se encontró el torneo con Id: {id}");
            }

            await _torneoCommand.RemoveTorneo(id);
        }

        public async Task<List<Torneo>> GetAllTorneos()
        {
            return await _torneoQuery.GetListTorneos();
        }

        public async Task<Torneo?> GetTorneoById(Guid id)
        {
            return await _torneoQuery.GetTorneoById(id);
        }

        public async Task InscribirCliente(Guid torneoId, Guid clienteId)
        {
            var torneo = await _torneoQuery.GetTorneoById(torneoId);
            if (torneo == null)
            {
                throw new KeyNotFoundException("El torneo no existe.");
            }

            if (torneo.Estado == EstadoTorneo.Cancelado || torneo.Estado == EstadoTorneo.Finalizado)
            {
                throw new InvalidOperationException("No se pueden realizar inscripciones en un torneo finalizado o cancelado.");
            }

            // RF-44: No inscribir 2 veces al mismo usuario
            var yaInscripto = await _torneoQuery.IsClienteInscripto(torneoId, clienteId);
            if (yaInscripto)
            {
                throw new InvalidOperationException("El usuario ya se encuentra inscripto en este torneo.");
            }

            // RF-44: Límite máximo de participantes
            var inscripciones = await _torneoQuery.GetInscripcionesByTorneo(torneoId);
            if (inscripciones.Count >= torneo.CantidadMaximaParticipantes)
            {
                throw new InvalidOperationException("Se ha alcanzado el límite máximo de participantes para este torneo.");
            }

            var inscripcion = new InscripcionTorneo
            {
                Id = Guid.NewGuid(),
                TorneoId = torneoId,
                ClienteId = clienteId,
                FechaInscripcion = DateTime.UtcNow
            };

            await _torneoCommand.InsertInscripcion(inscripcion);
        }

        public async Task GenerarFixture(Guid torneoId)
        {
            var torneo = await _torneoQuery.GetTorneoById(torneoId);
            if (torneo == null)
            {
                throw new KeyNotFoundException("El torneo no existe.");
            }

            var inscripciones = await _torneoQuery.GetInscripcionesByTorneo(torneoId);

            // RF-42: No permitir generar cuadro si no se alcanza el mínimo de participantes
            if (inscripciones.Count < torneo.CantidadMinimaParticipantes)
            {
                throw new InvalidOperationException($"No se alcanza la cantidad mínima requerida de participantes ({torneo.CantidadMinimaParticipantes}).");
            }

            // Obtener canchas habilitadas para el torneo según el tipo de cancha requerido
            var todasCanchas = await _canchaQuery.GetListCanchas();
            var canchasHabilitadas = todasCanchas
                .Where(c => c.Estado == EstadoCancha.Disponible && (torneo.TipoCanchaId == Guid.Empty || c.TipoCanchaId == torneo.TipoCanchaId))
                .ToList();

            if (canchasHabilitadas.Count == 0)
            {
                canchasHabilitadas = todasCanchas.Where(c => c.Estado == EstadoCancha.Disponible).ToList();
            }

            if (canchasHabilitadas.Count == 0)
            {
                throw new InvalidOperationException("No hay canchas disponibles para asignar a los partidos del torneo.");
            }

            // Generar cruces de primera ronda por parejas de inscriptos y asignar cancha válida
            var inscriptos = inscripciones.ToList();
            int matchIndex = 0;
            for (int i = 0; i < inscriptos.Count - 1; i += 2)
            {
                var canchaAsignada = canchasHabilitadas[matchIndex % canchasHabilitadas.Count];

                var partido = new Partido
                {
                    Id = Guid.NewGuid(),
                    TorneoId = torneoId,
                    Fecha = torneo.FechaInicio,
                    Instancia = "Primera Ronda",
                    Estado = EstadoPartido.Programado,
                    CanchaId = canchaAsignada.Id
                };

                if (inscriptos[i].Cliente != null) partido.Competidores.Add(inscriptos[i].Cliente);
                if (inscriptos[i + 1].Cliente != null) partido.Competidores.Add(inscriptos[i + 1].Cliente);

                await _torneoCommand.InsertPartido(partido);
                matchIndex++;
            }

            torneo.Estado = EstadoTorneo.EnCurso;
            await _torneoCommand.UpdateTorneo(torneo);
        }

        public async Task RegistrarResultadoPartido(Guid partidoId, string resultado)
        {
            var partido = await _torneoQuery.GetPartidoByIdAsync(partidoId);
            if (partido == null)
            {
                throw new NotFoundException($"No se encontró el partido con Id: {partidoId}");
            }

            partido.Resultado = resultado;
            partido.Estado = EstadoPartido.Finalizado;

            await _torneoCommand.UpdatePartido(partido);
        }

        public async Task<List<Partido>> GetFixtureTorneo(Guid torneoId)
        {
            return await _torneoQuery.GetPartidosByTorneo(torneoId);
        }
    }
}
