using Dominio.Entidades;
using System;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IBloqueoCanchaCommand
    {
        Task InsertBloqueo(BloqueoCancha bloqueo);
        Task RemoveBloqueo(Guid id);
    }
}