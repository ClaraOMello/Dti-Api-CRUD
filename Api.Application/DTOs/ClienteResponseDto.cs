using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.Application.DTOs
{
    public class ClienteResponseDto
    {
        [JsonPropertyName("dataInsercao")]
        public DateTime DataInsercao { get; set; }
         
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { set; get; }

        [JsonPropertyName("email")]
        public string Email { set; get; }

        [JsonPropertyName("cpf")]
        public string CPF { set; get; }

        [JsonPropertyName("idade")]
        public int Idade { set; get; }
    }
}
