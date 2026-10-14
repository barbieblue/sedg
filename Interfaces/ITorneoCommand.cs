using Dominio.Entidades;
using System;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface ITorneoCommand
    {
        Task InsertTorneo(Torneo torneo);
        Task UpdateTorneo(Torneo torneo);
        Task CancelTorneo(Guid id);
        Task RemoveTorneo(Guid id);
        Task InsertInscripcion(InscripcionTorneo inscripcion);
        Task InsertPartido(Partido partido);
        Task UpdatePartido(Partido partido);
    }
}
