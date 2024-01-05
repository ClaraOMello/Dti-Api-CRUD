using Api.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Interfaces
{
    public interface IProcessarClienteAppService
    {
        ClienteResponseDto BuscarCliente(ClienteRequestDto request);

        List<ClienteResponseDto> BuscarTodosClientes();

        ClienteResponseDto CriarNovoCliente(ClienteRequestDto request);

        ClienteResponseDto AtualizarCliente(ClienteRequestDto request);

        ClienteResponseDto ExcluirCliente(ClienteRequestDto request);
    }
}
