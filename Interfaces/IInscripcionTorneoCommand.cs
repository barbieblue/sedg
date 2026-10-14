using Dominio.Entidades;
using System;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IInscripcionTorneoCommand
    {
        Task InsertInscripcion(InscripcionTorneo inscripcion);
        Task RemoveInscripcion(Guid id);
    }
}