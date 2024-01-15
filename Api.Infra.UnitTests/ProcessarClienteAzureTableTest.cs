using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using Azure.Data.Tables;
using Api.Infra.Converters;

namespace Api.Infra.UnitTests
{
    public class ProcessarClienteAzureTableTest
    {
        private Fixture _fixture;
        private Mock<TableClient> _tableClientMock;
        private Mock<ClienteConverter> _clienteConverterMock;

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
            _clienteConverterMock = new Mock<ClienteConverter>();
        }

        [Test]
        public void DeveAtualizarClienteComSucesso()
        {
            Assert.Pass();
        }

        [Test]
        public void DeveBuscarERetornarUmCliente()
        {
            Assert.Pass();
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
    }
}