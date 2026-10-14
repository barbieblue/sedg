using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface ICanchaQuery
    {
        Task<List<Cancha>> GetListCanchas();
        Task<Cancha?> GetCanchaById(Guid id);
        Task<Cancha?> GetCanchaByNumero(int numero);
        Task<bool> ExistsNumero(int numero);
        Task<List<BloqueoCancha>> GetBloqueosByCancha(Guid canchaId);
        Task<bool> IsCanchaDisponible(Guid canchaId, DateTime fecha, TimeOnly horaInicio, TimeOnly horaFin);
    }
}
