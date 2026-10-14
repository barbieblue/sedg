using Aplicación.Interfaces;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicación.UseCase
{
    public class ClienteServices : IClienteServices
    {
        private readonly IClienteCommand _clienteCommand;
        private readonly IClienteQuery _clienteQuery;

        public ClienteServices(IClienteCommand clienteCommand, IClienteQuery clienteQuery)
        {
            _clienteCommand = clienteCommand;
            _clienteQuery = clienteQuery;
        }

        public Task CreateCliente(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCliente(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cliente>> GetAllClientes()
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> GetClienteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCliente(Cliente cliente)
        {
            throw new NotImplementedException();
        }
    }
}
