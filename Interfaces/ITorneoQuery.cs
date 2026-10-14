using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface ITorneoQuery
    {
        Task<List<Torneo>> GetListTorneos();
        Task<Torneo?> GetTorneoById(Guid id);
        Task<List<InscripcionTorneo>> GetInscripcionesByTorneo(Guid torneoId);
        Task<List<Partido>> GetPartidosByTorneo(Guid torneoId);
        Task<Partido?> GetPartidoByIdAsync(Guid partidoId);
        Task<bool> IsClienteInscripto(Guid torneoId, Guid clienteId);
    }
}
