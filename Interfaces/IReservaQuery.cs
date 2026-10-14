using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IReservaQuery
    {
        Task<List<Reserva>> GetListReservas();
        Task<Reserva?> GetReservaById(Guid id);
        Task<List<Reserva>> GetReservasByCliente(Guid clienteId);
        Task<List<Reserva>> GetReservasByFechaYCancha(DateOnly fecha, Guid? canchaId);
    }
}
