using Aplicación.Interfaces;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicación.UseCase
{
    public class InventarioServices : IInventarioServices
    {
        private readonly IInventarioCommand _inventarioCommand;
        private readonly IInventarioQuery _inventarioQuery;

        public InventarioServices(IInventarioCommand inventarioCommand, IInventarioQuery inventarioQuery)
        {
            _inventarioCommand = inventarioCommand;
            _inventarioQuery = inventarioQuery;
        }

        public async Task CreateItem(ItemInventario item)
        {
            var existente = await _inventarioQuery.GetItemByTipo(item.Tipo);
            if (existente != null)
            {
                throw new InvalidOperationException($"Ya existe un ítem registrado para el tipo '{item.Tipo}'.");
            }

            await _inventarioCommand.InsertItem(item);
        }

        public async Task UpdateItem(ItemInventario item)
        {
            await _inventarioCommand.UpdateItem(item);
        }

        public async Task DeleteItem(Guid id)
        {
            var item = await _inventarioQuery.GetItemById(id);
            if (item == null)
            {
                throw new KeyNotFoundException("El ítem de inventario no existe.");
            }

            await _inventarioCommand.DeleteItem(id);
        }

        public async Task<List<ItemInventario>> GetAllItems()
        {
            return await _inventarioQuery.GetListItems();
        }

        public async Task<ItemInventario?> GetItemById(Guid id)
        {
            return await _inventarioQuery.GetItemById(id);
        }

        public async Task<bool> VerificarStock(string tipo, int cantidad)
        {
            return await _inventarioQuery.HayStockDisponible(tipo, cantidad);
        }

        public async Task DescontarEquipamiento(Guid itemId, int cantidad)
        {
            await _inventarioCommand.DescontarStock(itemId, cantidad);
        }

        public async Task ReponerEquipamiento(Guid itemId, int cantidad)
        {
            await _inventarioCommand.DevolverStock(itemId, cantidad);
        }
    }
}
