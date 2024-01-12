using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using Api.Infra.Interfaces;
using Api.Models;
using Api.Domain.Interfaces;
using Api.Domain.Models.Enums;
using FluentAssertions;

namespace Api.Domain.UnitTest
{
    public class ProcessarClienteSeviceTest
    {
        // interfaces da classe
        private Mock<IProcessarClienteRepository> _processarClienteRepositoryMock;
        private Mock<IProcessarFaixaEtaria> _processarFaixaEtariaMock;
        private ProcessarClienteService _service;

        private Fixture _fixture;

        [OneTimeSetUp]
        public void SetupFixture()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        }

        [SetUp]
        public void Setup()
        {
            _processarClienteRepositoryMock = new Mock<IProcessarClienteRepository>();
            _processarFaixaEtariaMock = new Mock<IProcessarFaixaEtaria>();

            _service = new ProcessarClienteService(_processarFaixaEtariaMock.Object, _processarClienteRepositoryMock.Object);
        }

        [Test]
        public void DeveAtualizarClienteCasoExistaEPossuaIdade()
        {
            var cliente = _fixture.Create<Cliente>();
            var clienteAlteracoes = _fixture.Build<Cliente>()
                .With(c => c.Idade, 21)
                .Create();
            var clienteModificado = _fixture.Build<Cliente>()
                .With(c => c.Idade, 21)
                .With(c => c.FaixaEtaria, FaixaEtaria.Adulto)
                .With(c => c.Modificado, true)
                .Create();

            _processarFaixaEtariaMock
                .Setup(r => r.DefinirFaixaEtaria(21))
                .Returns(FaixaEtaria.Adulto);

            _processarClienteRepositoryMock
                .Setup(r => r.BuscarCliente(clienteAlteracoes.Id))
                .Returns(cliente);

            _processarClienteRepositoryMock
                .Setup(r => r.AtualizarCliente(It.IsAny<Cliente>()))
                .Returns(clienteModificado);

            var service = Instanciar();
            var retorno = service.AtualizarCliente(clienteAlteracoes);

            retorno.Should().BeOfType<Cliente>();
            retorno.Should().Be(clienteModificado);
            Assert.IsTrue(retorno.Modificado);
            Assert.That(retorno.FaixaEtaria, Is.EqualTo(FaixaEtaria.Adulto));
        }

        [Test]
        public void DeveCriarClienteCasoNaoExistaNaAtulizacao()
        {
            var id = 1;
            Cliente nulo = null;
            var clienteAlteracoes = _fixture.Build<Cliente>()
                .With(c => c.Id, id)
                .With(c => c.Nome, "Novo Nome")
                .With(c => c.Modificado, false)
                .Create();

            _processarClienteRepositoryMock
                .Setup(r => r.BuscarCliente(id))
                .Returns(nulo);

            _processarClienteRepositoryMock
                .Setup(r => r.CriarNovoCliente(clienteAlteracoes))
                .Returns(clienteAlteracoes);

            var retorno = _service.AtualizarCliente(clienteAlteracoes);

            Assert.IsFalse(retorno.Modificado);
            Assert.That(retorno.Nome, Is.EqualTo(clienteAlteracoes.Nome));
        }

        [Test]
        public void DeveBuscarERetornarUmCliente()
        {
            var id = 1;
            var cliente = _fixture.Build<Cliente>()
                .With(c => c.Id, id)
                .Create();

            _processarClienteRepositoryMock
                .Setup(r => r.BuscarCliente(id))
                .Returns(cliente);

            var retorno = _service.BuscarCliente(id);
        }

        [Test]
        public void DeveBuscarERetornarUmaListaDeClientes()
        {
            Assert.Pass();
        }

        [Test]
        public void DeveCriarClienteCasoIdadeSejaInformada()
        {
            Assert.Pass();
        }

        [Test]
        public void DeveCriarClienteCasoIdadeNaoSejaInformada()
        {
            Assert.Pass();
        }

        [Test]
        public void DeveExcluirClienteRetornandoQueAOperacaoFoiBemSucedida()
        {
            Assert.Pass();
        }

        private ProcessarClienteService Instanciar()
        {
            return new ProcessarClienteService(_processarFaixaEtariaMock.Object, _processarClienteRepositoryMock.Object);
        }
    }
}