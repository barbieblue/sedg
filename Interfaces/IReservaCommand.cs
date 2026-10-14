using Dominio.Entidades;
using System;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IReservaCommand
    {
        Task InsertReserva(Reserva reserva);
        Task UpdateReserva(Reserva reserva);
        Task CancelReserva(Guid id);
    }
}
