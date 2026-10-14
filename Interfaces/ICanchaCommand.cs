using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface ICanchaCommand
    {
        Task InsertCancha(Cancha cancha);
        Task UpdateCancha(Cancha cancha);
        Task DeleteCancha(Guid id);
        Task RemoveCancha(Guid id);
        Task InsertBloqueo(BloqueoCancha bloqueo);
        Task RemoveBloqueo(Guid id);
    }
}
