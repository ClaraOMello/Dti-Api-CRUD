using Api.Domain.Models.Enums;

namespace Api.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nome { set; get; }

        public string Email { set; get; }

        public string CPF { set; get; }

        public int? Idade { set; get; }

        public FaixaEtaria FaixaEtaria { set; get; }

        public bool Modificado { set; get; } 
    }
}
