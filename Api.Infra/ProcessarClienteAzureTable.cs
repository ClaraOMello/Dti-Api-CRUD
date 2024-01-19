using Api.Infra.Converters;
using Api.Domain.Interfaces;
using Api.Infra.TablesEntities;
using Api.Models;
using Azure;
using Azure.Data.Tables;
using Api.Shared;

namespace Api.Infra
{
    public class ProcessarClienteAzureTable : IProcessarClienteRepository
    {
        private readonly TableClient _tableClient;
        private readonly TableServiceClient _serviceClient;
        private readonly ITwoWayConverter<Cliente, ClienteEntity> _converter;

        public ProcessarClienteAzureTable(
            TableServiceClient serviceClient,
            ITwoWayConverter<Cliente, ClienteEntity> converter)
        {
            _serviceClient = serviceClient;
            _serviceClient.CreateTableIfNotExists("Cliente");
            _tableClient = _serviceClient.GetTableClient("Cliente");
            _converter = converter;
        }

        public Cliente AtualizarCliente(Cliente cliente)
        {
            cliente.Modificado = true;
            _tableClient.UpdateEntity(_converter.Convert(cliente), ETag.All);
            return BuscarCliente(cliente.Id);
        }

        public Cliente BuscarCliente(int id)
        {
            var entidade = _tableClient.GetEntity<ClienteEntity>(id.ToString(), "");
            return _converter.Convert(entidade);
        }

        public async Task<List<Cliente>> BuscarTodosClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            await foreach ( Page<ClienteEntity> page in _tableClient.QueryAsync<ClienteEntity>().AsPages() )
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
            int maxId = _tableClient.Query<ClienteEntity>()
                    .Select(c => c.Id)
                    .DefaultIfEmpty(0)
                    .Max();
            cliente.Id = maxId + 1;
            cliente.Modificado = false;

            try
            {
                _tableClient.AddEntity(_converter.Convert(cliente));
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
                _tableClient.DeleteEntity(id.ToString(), "");
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
