using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using Azure.Data.Tables;
using Api.Models;
using Api.Infra.TablesEntities;
using Azure;
using FluentAssertions;
using Api.Shared;

namespace Api.Infra.UnitTests
{
    public class ProcessarClienteAzureTableTest
    {
        private Fixture _fixture;
        private Mock<TableClient> _tableClientMock;
        private Mock<ITwoWayConverter<Cliente, ClienteEntity>> _clienteConverterMock;

        [OneTimeSetUp]
        public void SetupFixture()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoConfiguredMoqCustomization());
        }

        [SetUp]
        public void Setup()
        {
            _tableClientMock = new Mock<TableClient>();
            _clienteConverterMock = new Mock<ITwoWayConverter<Cliente, ClienteEntity>>();
        }

        [Test]
        public void DeveAtualizarClienteComSucesso()
        {
            var cliente = _fixture.Create<Cliente>();
            var clienteAtualizado = _fixture.Create<Cliente>();
            var entidade = _fixture.Create<ClienteEntity>();
            var entResponse = _fixture.Create<Response<ClienteEntity>>();

            _clienteConverterMock
                .Setup(r => r.Convert(It.IsAny<Cliente>()))
                .Returns(entidade);

            _tableClientMock
                .Setup(r => r.UpdateEntity(entidade, ETag.All, TableUpdateMode.Merge, default));

            _tableClientMock
                .Setup(r => r.GetEntity<ClienteEntity>(cliente.Id.ToString(), "", null, default))
                .Returns(entResponse);

            _clienteConverterMock
                .Setup(r => r.Convert(It.IsAny<ClienteEntity>()))
                .Returns(clienteAtualizado);

            var tableService = Instanciar();
            var retorno = tableService.AtualizarCliente(cliente);

            retorno.Should().Be(clienteAtualizado);
        }

        [Test]
        public void DeveBuscarERetornarUmCliente()
        {
            
        }

        [Test]
        public void DeveBuscarERetornarTodosClientes()
        {
            Assert.Pass();
        }

        [Test]
        public void DeveCriarClienteERetornarClienteComId()
        {
            Assert.Pass();
        }

        [Test]
        public void DeveExcluirClienteComSucesso()
        {
            Assert.Pass();
        }

        private ProcessarClienteAzureTable Instanciar()
        {
            return new ProcessarClienteAzureTable(_clienteConverterMock);
        }
    }
}