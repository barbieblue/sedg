using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IBloqueoCanchaQuery
    {
        Task<List<BloqueoCancha>> GetBloqueosByCancha(Guid canchaId);
        Task<BloqueoCancha?> GetBloqueoById(Guid id);
    }
}