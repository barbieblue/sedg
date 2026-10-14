using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IReservaServices
    {
        Task CreateReserva(Reserva reserva);
        Task UpdateReserva(Reserva reserva);
        Task CancelarReserva(Guid id);
        Task<List<Reserva>> GetAllReservas();
        Task<Reserva?> GetReservaById(Guid id);
        Task<List<Reserva>> GetReservasByCliente(Guid clienteId);
        Task<List<Reserva>> GetReservasFiltradas(DateOnly fecha, Guid? canchaId);
    }
}
