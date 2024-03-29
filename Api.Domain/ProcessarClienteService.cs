﻿using Api.Domain.Interfaces;
using Api.Models;

namespace Api.Domain
{
    public class ProcessarClienteService : IProcessarClienteService
    {
        private readonly IProcessarFaixaEtaria _processarFaixaEtaria;
        private readonly IProcessarClienteRepository _processarClienteRepository;
        public ProcessarClienteService(
            IProcessarFaixaEtaria processarFaixaEtaria,
            IProcessarClienteRepository processarClienteRepository)
        {
            _processarFaixaEtaria = processarFaixaEtaria;
            _processarClienteRepository = processarClienteRepository;
        }

        public Cliente AtualizarCliente(Cliente cliente)
        {
            if ( BuscarCliente(cliente.Id) != null )
            {
                if ( cliente.Idade.HasValue )
                {
                    cliente.FaixaEtaria = _processarFaixaEtaria.DefinirFaixaEtaria((int) cliente.Idade);
                }
                return _processarClienteRepository.AtualizarCliente(cliente);
            }
            return CriarNovoCliente(cliente);
        }

        public Cliente BuscarCliente(int id)
        {
            return _processarClienteRepository.BuscarCliente(id);
        }

        public List<Cliente> BuscarTodosClientes()
        {
            return _processarClienteRepository.BuscarTodosClientes().Result;
        }

        public Cliente CriarNovoCliente(Cliente cliente)
        {
            if ( cliente.Idade.HasValue )
            {
                cliente.FaixaEtaria = _processarFaixaEtaria.DefinirFaixaEtaria((int) cliente.Idade);
            }
            return _processarClienteRepository.CriarNovoCliente(cliente);
        }

        public bool ExcluirCliente(int id)
        {
            return _processarClienteRepository.ExcluirCliente(id);
        }
    }
}
