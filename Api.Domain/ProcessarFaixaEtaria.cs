using Api.Domain.Interfaces;
using Api.Domain.Models.Enums;

namespace Api.Domain
{
    public class ProcessarFaixaEtaria : IProcessarFaixaEtaria
    {
        public FaixaEtaria DefinirFaixaEtaria(int idade)
        {
            // tratamento de Exceptions (try catch) 
            FaixaEtaria faixaEtaria;
            if ( idade < 0 ) throw new Exception("Idade inválida");
            else if ( idade <= 10 ) faixaEtaria = FaixaEtaria.Crianca;
            else if ( idade <= 17 ) faixaEtaria = FaixaEtaria.Adolescente;
            else  faixaEtaria = FaixaEtaria.Adulto;

            return faixaEtaria;
        }
    }
}
