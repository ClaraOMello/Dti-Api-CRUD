using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces
{
    public interface IProcessarClienteService
    {
        Cliente BuscarCliente(int id);

        List<Cliente> BuscarTodosClientes();

        Cliente CriarNovoCliente(Cliente cliente);

        bool ExcluirCliente(int id);

        Cliente AtualizarCliente(Cliente cliente);
    }
}
