using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicación.Interfaces
{
    public interface IClienteCommand
    {
        Task InsertCliente(Cliente cliente);
        Task RemoveCliente(Guid id);
    }
}
