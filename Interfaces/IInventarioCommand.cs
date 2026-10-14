using Dominio.Entidades;
using System;
using System.Threading.Tasks;

namespace Aplicación.Interfaces
{
    public interface IInventarioCommand
    {
        Task InsertItem(ItemInventario item);
        Task UpdateItem(ItemInventario item);
        Task DeleteItem(Guid id);
        Task DescontarStock(Guid itemId, int cantidad);
        Task DevolverStock(Guid itemId, int cantidad);
    }
}
