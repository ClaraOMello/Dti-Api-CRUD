using NUnit.Framework;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using Api.Application.Interfaces;
using Api.Controllers;
using Api.Application.DTOs;
using FluentAssertions;

namespace Api.Host.UnitTests
{
    public class ClienteControllerTest
    {
        private Fixture _fixture;
        private Mock<IProcessarClienteAppService> _processarClienteAppServiceMock;

        [OneTimeSetUp]
        public void SetupFixture()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [SetUp]
        public void Setup()
        {
            _processarClienteAppServiceMock = new Mock<IProcessarClienteAppService>();
        }

        [Test]
        public void DeveAtualizarClienteComSucesso()
        {
            var requestDto = _fixture.Build<ClienteRequestDto>().Create();
            var responseDto = _fixture.Build<ClienteResponseDto>()
                .With(r => r.Modificado, true)
                .With(r => r.CPF, requestDto.CPF)
                .With(r => r.Nome, requestDto.Nome)
                .With(r => r.Email, requestDto.Email)
                .Create();

            _processarClienteAppServiceMock
                .Setup(r => r.AtualizarCliente(requestDto))
                .Returns(responseDto);

            var appService = Instanciar();

            var retorno = appService.Update(requestDto.Id, requestDto);

            Assert.That(retorno.Nome == responseDto.Nome);
            Assert.IsTrue(retorno.Modificado);
        }
        
        [Test]
        public void DeveBuscarERetornarUmCliente()
        {
            var response = _fixture.Build<ClienteResponseDto>().Create();

            _processarClienteAppServiceMock
                .Setup(r => r.BuscarCliente(It.IsAny<ClienteRequestDto>()))
                .Returns(response);

            var appService = Instanciar();
            var retorno = appService.GetById(response.Id.ToString());

            retorno.Should().Be(response);
        }

        [Test]
        public void DeveBuscarERetornarTodosClientes()
        {
            var response = _fixture.CreateMany<ClienteResponseDto>(4).ToList();

            _processarClienteAppServiceMock
                .Setup(r => r.BuscarTodosClientes())
                .Returns(response);

            var appService = Instanciar();
            var retorno = appService.GetAll();

            retorno.Should().BeEquivalentTo(response);
        }

        [Test]
        public void DeveCriarClienteERetornarClienteComId()
        {
            var requestDto = _fixture.Build<ClienteRequestDto>().Create();
            var responseDto = _fixture.Build<ClienteResponseDto>()
                .With(r => r.Modificado, false)
                .Create();

            _processarClienteAppServiceMock
                .Setup(r => r.CriarNovoCliente(requestDto))
                .Returns(responseDto);

            var appService = Instanciar();

            var retorno = appService.Create(requestDto);

            retorno.Should().Be(responseDto);
            Assert.IsFalse(retorno.Modificado);
        }
        
        [Test]
        public void DeveExcluirClienteComSucesso()
        {
            var response = _fixture.Build<ClienteResponseDto>().Create();

            _processarClienteAppServiceMock
                .Setup(r => r.ExcluirCliente(It.IsAny<ClienteRequestDto>()))
                .Returns(response);

            var appService = Instanciar();
            var retorno = appService.Delete(response.Id.ToString());

            retorno.Should().Be(response);
        }

        private ClienteController Instanciar()
        {
            return new ClienteController(_processarClienteAppServiceMock.Object);
        }
    }
}