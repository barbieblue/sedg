using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IInscripcionTorneoQuery
    {
        Task<List<InscripcionTorneo>> GetInscripcionesByTorneo(Guid torneoId);
        Task<bool> ExistsInscripcion(Guid torneoId, Guid clienteId);
    }
}