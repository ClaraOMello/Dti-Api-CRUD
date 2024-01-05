using Api.Infra.TablesEntities;
using Api.Models;
using Api.Shared;
using Azure;

namespace Api.Infra.Converters
{
    public class ClienteConverter : ITwoWayConverter<Cliente, ClienteEntity>
    {
        public Cliente Convert(ClienteEntity entidade)
        {
            return new Cliente
            {
                Id = entidade.Id,
                CPF = entidade.CPF,
                Email = entidade.Email,
                FaixaEtaria = entidade.FaixaEtaria,
                Idade = entidade.Idade,
                Nome = entidade.Nome,
                Modificado = entidade.Modificado,
            };
        }

        public ClienteEntity Convert(Cliente cliente)
        {
            return new ClienteEntity
            {
                Id = cliente.Id,
                Email = cliente.Email,
                Nome = cliente.Nome,
                Idade= cliente.Idade,
                FaixaEtaria = cliente.FaixaEtaria,
                CPF= cliente.CPF,
                Modificado = cliente.Modificado,
                PartitionKey = cliente.Id.ToString(),
                RowKey = "",
                Timestamp = DateTime.Now,
                ETag = ETag.All
            };
        }
    }
}
