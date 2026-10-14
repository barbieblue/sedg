using Dominio.Entidades;
using System;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IPartidoCommand
    {
        Task InsertPartido(Partido partido);
        Task UpdatePartido(Partido partido);
        Task RemovePartido(Guid id);
    }
}