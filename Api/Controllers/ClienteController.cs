using Api.Application.DTOs;
using Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class ClienteController : ControllerBase
    {
        private readonly IProcessarClienteAppService _processarClienteAppService;

        public ClienteController(IProcessarClienteAppService processarClienteAppService)
        {
            _processarClienteAppService = processarClienteAppService;
        }

        [HttpGet("Cliente")]
        public ClienteResponseDto GetById([FromQuery] string id)
        {
            int.TryParse(id, out var intId);
            ClienteRequestDto req = new ClienteRequestDto { Id = intId };
            return _processarClienteAppService.BuscarCliente(req);
        }

        [HttpGet("Clientes")]
        public List<ClienteResponseDto> GetAll()
        {
            return _processarClienteAppService.BuscarTodosClientes();
        }

        [HttpPost("Cliente")]
        public ClienteResponseDto Create([FromBody] ClienteRequestDto request)
        {
            return _processarClienteAppService.CriarNovoCliente(request);
        }

        [HttpPut("Cliente/{id}")]
        public ClienteResponseDto Update(int id, [FromBody] ClienteRequestDto request)
        {
            request.Id = id;
            return _processarClienteAppService.AtualizarCliente(request);
        }

        [HttpDelete("Cliente")]
        public ClienteResponseDto Delete([FromQuery] string id)
        {
            int.TryParse(id, out var intId);
            ClienteRequestDto req = new ClienteRequestDto { Id = intId };
            return _processarClienteAppService.ExcluirCliente(req);
        }

    }
}
