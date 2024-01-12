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
        public void DeveAtualizarClienteCasoExistaENaoPossuaIdade()
        {
            int? idade = null;
            var cliente = _fixture.Create<Cliente>();
            var clienteAlteracoes = _fixture.Build<Cliente>()
                .With(c => c.Idade, idade)
                .Create();
            var clienteModificado = _fixture.Build<Cliente>()
                .With(c => c.Idade, idade)
                .With(c => c.Modificado, true)
                .Create();

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
            Assert.That(retorno.Idade, Is.EqualTo(null));
        }

        [Test]
        public void DeveCriarClienteCasoNaoExistaNaAtulizacao()
        {
            var cliente = _fixture.Build<Cliente>()
                .With(c => c.Modificado, false)
                .Create();
            Cliente nulo = null;

            _processarClienteRepositoryMock
                .Setup(r => r.BuscarCliente(cliente.Id))
                .Returns(nulo);

            _processarClienteRepositoryMock
                .Setup(r => r.CriarNovoCliente(cliente))
                .Returns(cliente);

            var service = Instanciar();
            var retorno = service.AtualizarCliente(cliente);

            Assert.IsFalse(retorno.Modificado);
            retorno.Should().BeEquivalentTo(cliente);
        }

        [Test]
        public void DeveBuscarERetornarUmCliente()
        {
            var cliente = _fixture.Build<Cliente>()
                .Create();

            _processarClienteRepositoryMock
                .Setup(r => r.BuscarCliente(cliente.Id))
                .Returns(cliente);

            var service = Instanciar();
            var retorno = service.BuscarCliente(cliente.Id);
        }

        [Test]
        public void DeveBuscarERetornarUmaListaDeClientes()
        {
            var clientes = _fixture.CreateMany<Cliente>(4).ToList();

            _processarClienteRepositoryMock
                .Setup(r => r.BuscarTodosClientes())
                .Returns(Task.FromResult(clientes));

            var service = Instanciar();
            var retorno = service.BuscarTodosClientes();
        }

        [Test]
        public void DeveCriarClienteCasoIdadeSejaInformada()
        {
            var cliente = _fixture.Build<Cliente>()
                .With(c => c.Idade, 6)
                .With(c => c.FaixaEtaria, FaixaEtaria.Adolescente)
                .With(c => c.Modificado, false)
                .Create();

            _processarFaixaEtariaMock
                .Setup(r => r.DefinirFaixaEtaria(6))
                .Returns(FaixaEtaria.Crianca);

            _processarClienteRepositoryMock
                .Setup(r => r.CriarNovoCliente(cliente))
                .Returns(cliente);

            var service = Instanciar();
            var retorno = service.CriarNovoCliente(cliente);

            retorno.Should().Be(cliente);
            Assert.IsFalse(retorno.Modificado);
            Assert.That(retorno.FaixaEtaria, Is.EqualTo(FaixaEtaria.Crianca));
        }

        [Test]
        public void DeveCriarClienteCasoIdadeNaoSejaInformada()
        {
            int? idade = null;
            var cliente = _fixture.Build<Cliente>()
                .With(c => c.Idade, idade)
                .With(c => c.Modificado, false)
                .Create();

            _processarClienteRepositoryMock
                .Setup(r => r.CriarNovoCliente(cliente))
                .Returns(cliente);

            var service = Instanciar();
            var retorno = service.CriarNovoCliente(cliente);

            retorno.Should().Be(cliente);
            Assert.IsFalse(retorno.Modificado);
        }

        [Test]
        public void DeveExcluirClienteComSucesso()
        {
            var id = 1;
            _processarClienteRepositoryMock
                .Setup(r => r.ExcluirCliente(id))
                .Returns(true);

            var service = Instanciar();
            var retorno = service.ExcluirCliente(id);

            Assert.IsTrue(retorno);
        }

        private ProcessarClienteService Instanciar()
        {
            return new ProcessarClienteService(_processarFaixaEtariaMock.Object, _processarClienteRepositoryMock.Object);
        }
    }
}