using System.Text.Json.Serialization;

namespace Api.Application.DTOs
{
    public class ClienteRequestDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string? Nome { set; get; }

        [JsonPropertyName("email")]
        public string? Email { set; get; }

        [JsonPropertyName("cpf")]
        public string? CPF { set; get; }

        [JsonPropertyName("idade")]
        public int? Idade{ set; get; }

    }
}
