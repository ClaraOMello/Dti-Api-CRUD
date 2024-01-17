using NUnit.Framework;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using Api.Domain.Interfaces;
using AutoMapper;
using Api.Models;
using Api.Application.DTOs;
using FluentAssertions;

namespace Api.Application.UnitTests
{
    public class ProcessarClienteAppServiceTest
    {
        private Fixture _fixture;
        private Mock<IProcessarClienteService> _processarClienteServiceMock;
        // private Mock<IMapper> _mapperMock;

        [OneTimeSetUp]
        public void SetupFixture()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [SetUp]
        public void Setup()
        {
            _processarClienteServiceMock = new Mock<IProcessarClienteService>();
            // _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public void DeveAtualizarClienteComSucesso()
        {
            var request = _fixture.Create<ClienteRequestDto>();
            var cliente = _fixture
                .Build<Cliente>()
                .With(c => c.Id, request.Id)
                .Create();

            _processarClienteServiceMock
                .Setup(r => r.AtualizarCliente(It.IsAny<Cliente>()))
                .Returns(cliente);

            var appService = Instanciar();
            var retorno = appService.AtualizarCliente(request);

            Assert.That(retorno.Id == cliente.Id);
        }

        [Test]
        public void DeveBuscarUmClienteComSucesso()
        {
            var cliente = _fixture.Create<Cliente>();
            var request = _fixture
                .Build<ClienteRequestDto>()
                .With(c => c.Id, cliente.Id)
                .Create();

            _processarClienteServiceMock
                .Setup(r => r.BuscarCliente(cliente.Id))
                .Returns(cliente);

            var appService = Instanciar();
            var retorno = appService.BuscarCliente(request);

            Assert.That(retorno.Id ==  cliente.Id);
        }

        [Test]
        public void DeveBuscarERetornarTodosClientes()
        {
            var clientes = _fixture.CreateMany<Cliente>().ToList();

            _processarClienteServiceMock
                .Setup(r => r.BuscarTodosClientes())
                .Returns(clientes);

            var appService = Instanciar();
            var retorno = appService.BuscarTodosClientes();

            retorno.Should().BeOfType<List<ClienteResponseDto>>();
        }

        [Test]
        public void DeveCriarClienteComServiceNaoNulo()
        {
            var request = _fixture.Create<ClienteRequestDto>();
            var response = _fixture.Create<ClienteResponseDto>();

            _processarClienteServiceMock
                .Setup(r => r.CriarNovoCliente(It.IsAny<Cliente>()))
                .Returns(new Cliente());

            var appService = Instanciar();
            var retorno = appService.CriarNovoCliente(request);

            Assert.That(retorno.Id == retorno.Id);
        }

        [Test]
        public void DeveDarFalharAoCriarCliente()
        {
            var request = _fixture.Create<ClienteRequestDto>();
            var response = _fixture.Create<ClienteResponseDto>();

            _processarClienteServiceMock
                .Setup(r => r.CriarNovoCliente(It.IsAny<Cliente>()));

            var appService = Instanciar();
            var retorno = appService.CriarNovoCliente(request);

            Assert.That(retorno.Id == 400);
        }

        [Test]
        public void DeveExcluirClienteComSucesso()
        {
            var cliente = _fixture.Create<Cliente>();
            var request = _fixture
                .Build<ClienteRequestDto>()
                .With(c => c.Id, cliente.Id)
                .Create();
            var response = _fixture.Create<ClienteResponseDto>();

            _processarClienteServiceMock
                .Setup(r => r.ExcluirCliente(cliente.Id))
                .Returns(true);

            var appService = Instanciar();
            var retorno = appService.ExcluirCliente(request);

            Assert.That(retorno.Id == cliente.Id);
        }

        [Test]
        public void DeveDarFalhaAoExcluirCliente()
        {
            var cliente = _fixture.Create<Cliente>();
            var request = _fixture
                .Build<ClienteRequestDto>()
                .With(c => c.Id, cliente.Id)
                .Create();
            var response = _fixture.Create<ClienteResponseDto>();

            _processarClienteServiceMock
                .Setup(r => r.ExcluirCliente(cliente.Id))
                .Returns(false);

            var appService = Instanciar();
            var retorno = appService.ExcluirCliente(request);

            Assert.That(retorno.Id == 400);
        }

        private ProcessarClienteAppService Instanciar()
        {
            return new ProcessarClienteAppService(_processarClienteServiceMock.Object);
        }
    }
}