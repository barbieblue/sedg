using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IPartidoServices
    {
        Task CreatePartido(Partido partido);
        Task UpdatePartido(Partido partido);
        Task DeletePartido(Guid id);
        Task<List<Partido>> GetPartidosByTorneo(Guid torneoId);
        Task<Partido?> GetPartidoById(Guid id);
        Task RegistrarResultado(Guid partidoId, string resultado);
    }
}