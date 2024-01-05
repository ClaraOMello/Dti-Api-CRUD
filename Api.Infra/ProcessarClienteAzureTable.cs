using Api.Infra.Converters;
using Api.Infra.Interfaces;
using Api.Infra.TablesEntities;
using Api.Models;
using Azure;
using Azure.Data.Tables;

namespace Api.Infra
{
    public class ProcessarClienteAzureTable : IProcessarClienteRepository
    {
        private readonly TableClient _tabelaCliente;
        private readonly ClienteConverter _converter;

        public ProcessarClienteAzureTable()
        {
            _tabelaCliente = new TableClient("UseDevelopmentStorage=true", "Cliente");
            _converter = new ClienteConverter();
        }

        public Cliente AtualizarCliente(Cliente cliente)
        {
            cliente.Modificado = true;
            _tabelaCliente.UpdateEntity(_converter.Convert(cliente), ETag.All);
            return BuscarCliente(cliente.Id);
        }

        public Cliente BuscarCliente(int id)
        {
            var entidade = _tabelaCliente.GetEntity<ClienteEntity>(id.ToString(), "");
            return _converter.Convert(entidade);
        }

        public async Task<List<Cliente>> BuscarTodosClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            await foreach ( Page<ClienteEntity> page in _tabelaCliente.QueryAsync<ClienteEntity>().AsPages() )
            {
                foreach ( ClienteEntity c in page.Values )
                {
                    clientes.Add(_converter.Convert(c));
                }
            }

            return clientes;
        }

        public Cliente CriarNovoCliente(Cliente cliente)
        {
            int maxId = _tabelaCliente.Query<ClienteEntity>()
                    .Select(c => c.Id)
                    .DefaultIfEmpty(0)
                    .Max();
            cliente.Id = maxId + 1;
            cliente.Modificado = false;

            try
            {
                _tabelaCliente.AddEntity(_converter.Convert(cliente));
            }
            catch ( Exception e )
            {
                Console.WriteLine(e);
                cliente = null;
            }
            return cliente;
        }

        public bool ExcluirCliente(int id)
        {
            try
            {
                _tabelaCliente.DeleteEntity(id.ToString(), "");
                return true;
            }
            catch ( Exception e )
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
