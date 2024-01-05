using Api.Models;

namespace Api.Infra.Interfaces
{
    public interface IProcessarClienteRepository
    {
        Cliente BuscarCliente(int id);

        Task<List<Cliente>> BuscarTodosClientes();

        Cliente CriarNovoCliente(Cliente cliente);

        bool ExcluirCliente(int id);

        Cliente AtualizarCliente(Cliente cliente);
    }
}
