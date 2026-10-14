using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface ICanchaServices
    {
        Task CreateCancha(Cancha cancha);
        Task UpdateCancha(Cancha cancha);
        Task DeleteCancha(Guid id);
        Task<List<Cancha>> GetAllCanchas();
        Task<Cancha?> GetCanchaById(Guid id);
        Task<Cancha?> GetCanchaByNumero(int numero);
        Task BloquearCancha(Guid canchaId, DateTime fechaInicio, DateTime fechaFin, string motivo);
        Task DesbloquearCancha(Guid bloqueoId);
        Task<bool> ConsultarDisponibilidad(Guid canchaId, DateTime fecha, TimeOnly horaInicio, TimeOnly horaFin);
    }
}
