using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface ITorneoServices
    {
        Task CreateTorneo(Torneo torneo);
        Task UpdateTorneo(Torneo torneo);
        Task CancelarTorneo(Guid id);
        Task DeleteTorneo(Guid id);
        Task<List<Torneo>> GetAllTorneos();
        Task<Torneo?> GetTorneoById(Guid id);
        Task InscribirCliente(Guid torneoId, Guid clienteId);
        Task GenerarFixture(Guid torneoId);
        Task RegistrarResultadoPartido(Guid partidoId, string resultado);
        Task<List<Partido>> GetFixtureTorneo(Guid torneoId);
    }
}
