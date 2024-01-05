using Api.Domain.Models.Enums;
using Azure;
using Azure.Data.Tables;

namespace Api.Infra.TablesEntities
{
    public class ClienteEntity : ITableEntity
    {
        public int Id { get; set; }

        public string Nome { set; get; }

        public string Email { set; get; }

        public string CPF { set; get; }

        public int? Idade { set; get; }

        public FaixaEtaria FaixaEtaria { set; get; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
