using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicación.Interfaces
{
    public interface IClienteQuery
    {
        Task<List<Cliente>> GetListClientes();
        Task<Cliente?> GetClienteById(Guid id);
    }
}
