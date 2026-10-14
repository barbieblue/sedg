using Aplicación.Interfaces;
using Dominio.Entidades;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.UseCase
{
    public class ReservaServices : IReservaServices
    {
        private readonly IReservaCommand _reservaCommand;
        private readonly IReservaQuery _reservaQuery;
        private readonly ICanchaQuery _canchaQuery;

        public ReservaServices(IReservaCommand reservaCommand, IReservaQuery reservaQuery, ICanchaQuery canchaQuery)
        {
            _reservaCommand = reservaCommand;
            _reservaQuery = reservaQuery;
            _canchaQuery = canchaQuery;
        }

        public async Task CreateReserva(Reserva reserva)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            // Regla: No fechas anteriores a la actual
            if (reserva.Fecha < hoy)
            {
                throw new ArgumentException("No se pueden registrar reservas con fechas anteriores a la actual.");
            }

            // Regla: Antelación máxima de 30 días
            if (reserva.Fecha > hoy.AddDays(30))
            {
                throw new ArgumentException("No se permiten reservas con más de 30 días de anticipación.");
            }

            // Regla: Duración máxima de 2 horas
            var duracion = reserva.HoraFin - reserva.HoraInicio;
            if (duracion > TimeSpan.FromHours(2))
            {
                throw new ArgumentException("Cada alquiler debe tener una duración máxima de 2 horas.");
            }

            if (duracion <= TimeSpan.Zero)
            {
                throw new ArgumentException("La hora de finalización debe ser posterior a la hora de inicio.");
            }

            // Regla: Verificar disponibilidad de la cancha
            var fechaDateTime = reserva.Fecha.ToDateTime(TimeOnly.MinValue);
            var disponible = await _canchaQuery.IsCanchaDisponible(reserva.CanchaId, fechaDateTime, reserva.HoraInicio, reserva.HoraFin);
            if (!disponible)
            {
                throw new InvalidOperationException("La cancha seleccionada no se encuentra disponible en dicho horario.");
            }

            // Regla: Participantes entre 2 y 4 personas
            if (reserva.Participantes != null && (reserva.Participantes.Count < 2 || reserva.Participantes.Count > 4))
            {
                throw new ArgumentException("La cancha debe ser reservada para DOS o CUATRO personas.");
            }

            reserva.Estado = EstadoReserva.Pendiente;
            await _reservaCommand.InsertReserva(reserva);
        }

        public async Task UpdateReserva(Reserva reserva)
        {
            await _reservaCommand.UpdateReserva(reserva);
        }

        public async Task CancelarReserva(Guid id)
        {
            var reserva = await _reservaQuery.GetReservaById(id);
            if (reserva == null)
            {
                throw new KeyNotFoundException("La reserva no existe.");
            }

            await _reservaCommand.CancelReserva(id);
        }

        public async Task<List<Reserva>> GetAllReservas()
        {
            return await _reservaQuery.GetListReservas();
        }

        public async Task<Reserva?> GetReservaById(Guid id)
        {
            return await _reservaQuery.GetReservaById(id);
        }

        public async Task<List<Reserva>> GetReservasByCliente(Guid clienteId)
        {
            return await _reservaQuery.GetReservasByCliente(clienteId);
        }

        public async Task<List<Reserva>> GetReservasFiltradas(DateOnly fecha, Guid? canchaId)
        {
            return await _reservaQuery.GetReservasByFechaYCancha(fecha, canchaId);
        }
    }
}
