using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IPartidoQuery
    {
        Task<List<Partido>> GetPartidosByTorneo(Guid torneoId);
        Task<Partido?> GetPartidoById(Guid id);
    }
}