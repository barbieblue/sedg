using Dominio.Entidades;

using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicación.Interfaces
{
    public interface IClienteServices
    {
        Task CreateCliente(Cliente cliente);
        Task UpdateCliente(Cliente cliente);
        Task DeleteCliente(Guid id);
        Task<List<Cliente>> GetAllClientes();
        Task<Cliente> GetClienteById(Guid id);
    }
}
