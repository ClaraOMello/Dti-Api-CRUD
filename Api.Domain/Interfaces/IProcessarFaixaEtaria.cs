using Api.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces
{
    public interface IProcessarFaixaEtaria
    {
        FaixaEtaria DefinirFaixaEtaria(int idade);
    }
}
