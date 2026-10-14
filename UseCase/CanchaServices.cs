using Aplicación.Interfaces;
using Dominio.Entidades;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.UseCase
{
    public class CanchaServices : ICanchaServices
    {
        private readonly ICanchaCommand _canchaCommand;
        private readonly ICanchaQuery _canchaQuery;

        public CanchaServices(ICanchaCommand canchaCommand, ICanchaQuery canchaQuery)
        {
            _canchaCommand = canchaCommand;
            _canchaQuery = canchaQuery;
        }

        public async Task CreateCancha(Cancha cancha)
        {
            var existente = await _canchaQuery.GetCanchaByNumero(cancha.Numero);
            if (existente != null)
            {
                throw new InvalidOperationException($"Ya existe una cancha con el número {cancha.Numero}.");
            }

            await _canchaCommand.InsertCancha(cancha);
        }

        public async Task UpdateCancha(Cancha cancha)
        {
            await _canchaCommand.UpdateCancha(cancha);
        }

        public async Task DeleteCancha(Guid id)
        {
            var cancha = await _canchaQuery.GetCanchaById(id);
            if (cancha == null)
            {
                throw new KeyNotFoundException("La cancha solicitada no existe.");
            }

            // Regla de integridad: No eliminar si tiene reservas asociadas
            if (cancha.Reservas != null && cancha.Reservas.Count > 0)
            {
                throw new InvalidOperationException("No se puede eliminar una cancha con reservas vinculadas.");
            }

            await _canchaCommand.DeleteCancha(id);
        }

        public async Task<List<Cancha>> GetAllCanchas()
        {
            return await _canchaQuery.GetListCanchas();
        }

        public async Task<Cancha?> GetCanchaById(Guid id)
        {
            return await _canchaQuery.GetCanchaById(id);
        }

        public async Task<Cancha?> GetCanchaByNumero(int numero)
        {
            return await _canchaQuery.GetCanchaByNumero(numero);
        }

        public async Task BloquearCancha(Guid canchaId, DateTime fechaInicio, DateTime fechaFin, string motivo)
        {
            var cancha = await _canchaQuery.GetCanchaById(canchaId);
            if (cancha == null)
            {
                throw new KeyNotFoundException("La cancha no existe.");
            }

            var bloqueo = new BloqueoCancha
            {
                Id = Guid.NewGuid(),
                CanchaId = canchaId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Motivo = motivo
            };

            await _canchaCommand.InsertBloqueo(bloqueo);
        }

        public async Task DesbloquearCancha(Guid bloqueoId)
        {
            await _canchaCommand.RemoveBloqueo(bloqueoId);
        }

        public async Task<bool> ConsultarDisponibilidad(Guid canchaId, DateTime fecha, TimeOnly horaInicio, TimeOnly horaFin)
        {
            return await _canchaQuery.IsCanchaDisponible(canchaId, fecha, horaInicio, horaFin);
        }
    }
}
