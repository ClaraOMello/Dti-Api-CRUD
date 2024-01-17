using Api.Application.DTOs;
using Api.Application.Interfaces;
using Api.Domain.Interfaces;
using Api.Models;
using AutoMapper;

namespace Api.Application
{
    public class ProcessarClienteAppService : IProcessarClienteAppService
    {
        private readonly IProcessarClienteService _processarClienteService;
        private readonly IMapper _mapper;
        public ProcessarClienteAppService(IProcessarClienteService processarClienteService)
        {
            _processarClienteService = processarClienteService;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cliente, ClienteResponseDto>();
                cfg.CreateMap<ClienteRequestDto, Cliente>()
                    .ForMember(dest => dest.Idade, opt => opt.Condition(src => ( src.Idade.HasValue )));
            });
            _mapper = configuration.CreateMapper();
        }

        public ClienteResponseDto BuscarCliente(ClienteRequestDto request)
        {
            var retorno = _processarClienteService.BuscarCliente(request.Id);
            return _mapper.Map<ClienteResponseDto>(retorno);
        }

        public List<ClienteResponseDto> BuscarTodosClientes()
        {
            var response = new List<ClienteResponseDto> { };
            var retorno = _processarClienteService.BuscarTodosClientes();
            foreach ( var cliente in retorno )
            {
                response.Add(
                    _mapper.Map<ClienteResponseDto>(cliente)
                );
            }
            return response;
        }

        public ClienteResponseDto AtualizarCliente(ClienteRequestDto request)
        {
            var cliente = _mapper.Map<Cliente>(request);

            var retorno = _processarClienteService.AtualizarCliente(cliente);

            var resp = _mapper.Map<ClienteResponseDto>(retorno);
            resp.DataInsercao = DateTime.Now;
            return resp;
        }

        public ClienteResponseDto CriarNovoCliente(ClienteRequestDto request)
        {
            var cliente = _mapper.Map<Cliente>(request);

            var retorno = _processarClienteService.CriarNovoCliente(cliente);
            if ( retorno == null )
            {
                return new ClienteResponseDto
                {
                    Id = 400,
                    Nome = "!! Fail !!"
                };
            }
            var resp = _mapper.Map<ClienteResponseDto>(retorno);
            resp.DataInsercao = DateTime.Now;
            return resp;
        }

        public ClienteResponseDto ExcluirCliente(ClienteRequestDto request)
        {
            var retorno = _processarClienteService.ExcluirCliente(request.Id);
            if ( retorno )
            {
                return new ClienteResponseDto
                {
                    Id = request.Id,
                    Nome = "Excluiu"
                };
            }
            else
            {
                return new ClienteResponseDto
                {
                    Id = 400,
                    Nome = "Não excluiu"
                };
            }
        }
    }
}
