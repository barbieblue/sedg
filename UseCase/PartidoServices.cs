using Aplicación.Interfaces;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Dominio.Entidades.EnumeracionesEstados;

namespace Aplicación.UseCase
{
    public class PartidoServices : IPartidoServices
    {
        private readonly IPartidoCommand _partidoCommand;
        private readonly IPartidoQuery _partidoQuery;

        public PartidoServices(IPartidoCommand partidoCommand, IPartidoQuery partidoQuery)
        {
            _partidoCommand = partidoCommand;
            _partidoQuery = partidoQuery;
        }

        public async Task CreatePartido(Partido partido)
        {
            await _partidoCommand.InsertPartido(partido);
        }

        public async Task UpdatePartido(Partido partido)
        {
            var existente = await _partidoQuery.GetPartidoById(partido.Id);
            if (existente == null)
            {
                throw new KeyNotFoundException($"No se encontró el partido con Id: {partido.Id}");
            }

            await _partidoCommand.UpdatePartido(partido);
        }

        public async Task DeletePartido(Guid id)
        {
            var existente = await _partidoQuery.GetPartidoById(id);
            if (existente == null)
            {
                throw new KeyNotFoundException($"No se encontró el partido con Id: {id}");
            }

            await _partidoCommand.RemovePartido(id);
        }

        public async Task<List<Partido>> GetPartidosByTorneo(Guid torneoId)
        {
            return await _partidoQuery.GetPartidosByTorneo(torneoId);
        }

        public async Task<Partido?> GetPartidoById(Guid id)
        {
            return await _partidoQuery.GetPartidoById(id);
        }

        public async Task RegistrarResultado(Guid partidoId, string resultado)
        {
            var partido = await _partidoQuery.GetPartidoById(partidoId);
            if (partido == null)
            {
                throw new KeyNotFoundException($"No se encontró el partido con Id: {partidoId}");
            }

            partido.Resultado = resultado;
            partido.Estado = EstadoPartido.Finalizado;

            await _partidoCommand.UpdatePartido(partido);
        }
    }
}
