using NUnit.Framework;
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
        private Mock<TableServiceClient> _tableServiceClientMock;
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
            _tableServiceClientMock = new Mock<TableServiceClient>();
            _tableClientMock = new Mock<TableClient>();
            _clienteConverterMock = new Mock<ITwoWayConverter<Cliente, ClienteEntity>>();

            _tableServiceClientMock
                .Setup(r => r.CreateTableIfNotExists("Cliente", default));

            _tableServiceClientMock
                .Setup(r => r.GetTableClient("Cliente"))
                .Returns(_tableClientMock.Object);
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
            var cliente = _fixture.Create<Cliente>();
            var entResponse = _fixture.Create<Response<ClienteEntity>>();

            _tableClientMock
                .Setup(r => r.GetEntity<ClienteEntity>(
                                cliente.Id.ToString(), 
                                "", 
                                null,
                                default))
                .Returns(entResponse);

            _clienteConverterMock
                .Setup(r => r.Convert(It.IsAny<ClienteEntity>()))
                .Returns(cliente);

            var tableService = Instanciar();
            var retorno = tableService.BuscarCliente(cliente.Id);

            retorno.Should().Be(cliente);
        }

        [Test]
        public void DeveBuscarERetornarTodosClientes()
        {
            var clienteEntities = _fixture.CreateMany<Cliente>();
            var pageableAsync = _fixture.Create<PageableAsyncClienteEntity>();
            var cliente = _fixture.Create<Cliente>();
            var clientes = new List<Cliente>();

            for (int i = 0; i< pageableAsync.GetItemsSize() ; i++ )
            {
                clientes.Add(cliente);
            }

            _tableClientMock
                .Setup(r => r.QueryAsync<ClienteEntity>(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<IEnumerable<string>>(), default))
                .Returns(pageableAsync);

            _clienteConverterMock
                .Setup(r => r.Convert(It.IsAny<ClienteEntity>()))
                .Returns(cliente);

            var appService = Instanciar();
            var retorno = appService.BuscarTodosClientes();

            retorno.Result.Should().BeEquivalentTo(clientes);
        }

        [Test]
        public void DeveCriarClienteERetornarClienteComId()
        {
            var cliente = _fixture.Create<Cliente>();
            var entidade = _fixture.Build<ClienteEntity>()
                .With(c => c.Id, 1)
                .Create();
            var pageable = _fixture.Create<PageableClienteEntity>();

            _tableClientMock
                .Setup(r => r.Query<ClienteEntity>(
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<IEnumerable<string>>(),
                    default))
                .Returns(pageable);
            
            _clienteConverterMock
                .Setup(r => r.Convert(It.IsAny<Cliente>()))
                .Returns(entidade);

            _tableClientMock.Setup(r => r.AddEntity(entidade, default));

            var appService = Instanciar();
            var retorno = appService.CriarNovoCliente(cliente);

            Assert.That(retorno.Id == entidade.Id);
        }

        [Test]
        public void DeveDarExcecaoAoCriarCliente()
        {
            var cliente = _fixture.Create<Cliente>();
            var pageable = _fixture.Create<PageableClienteEntity>();

            _tableClientMock
                .Setup(r => r.Query<ClienteEntity>(
                    It.IsAny<string>(),
                    It.IsAny<int?>(),
                    It.IsAny<IEnumerable<string>>(),
                    default))
                .Returns(pageable);

            _tableClientMock
                .Setup(r => r.AddEntity(It.IsAny<ClienteEntity>(), default))
                .Throws(new Exception());

            var appService = Instanciar();
            var retorno = appService.CriarNovoCliente(cliente);

            Assert.IsNull(retorno);
        }

        [Test]
        public void DeveExcluirClienteComSucesso()
        {
            var cliente = _fixture.Create<Cliente>();

            _tableClientMock
                .Setup(r => r.DeleteEntity(cliente.Id.ToString(), "", default, default));

            var tableService = Instanciar();
            var retorno = tableService.ExcluirCliente(cliente.Id);

            Assert.IsTrue(retorno);
        }

        [Test]
        public void DeveDarExcecaoAoExcluirCliente()
        {
            _tableClientMock
                .Setup(r => r.DeleteEntity(It.IsAny<string>(), "", default, default))
                .Throws(new Exception());

            var tableService = Instanciar();
            var retorno = tableService.ExcluirCliente(It.IsAny<int>());

            Assert.IsFalse(retorno);
        }

        private ProcessarClienteAzureTable Instanciar()
        {
            return new ProcessarClienteAzureTable(_tableServiceClientMock.Object, _clienteConverterMock.Object);
        }
    }

    public class PageableClienteEntity : Pageable<ClienteEntity>
    {
        private readonly IReadOnlyList<ClienteEntity> _items;

        public PageableClienteEntity(IReadOnlyList<ClienteEntity> items)
        {
            _items = new List<ClienteEntity>();
            foreach (var cliente in items)
            {
                cliente.Id = 0;
                _items.Append(cliente);
            }
        }

        public override IEnumerable<Page<ClienteEntity>> AsPages(string continuationToken = null, int? pageSizeHint = null)
        {
            Mock<Response> responseMock = new Mock<Response>();
            responseMock.SetupGet(r => r.Status).Returns(200);

            Response response = responseMock.Object;

            var page = Page<ClienteEntity>.FromValues(_items, continuationToken, response);
            yield return page;
        }
    }

    public class PageableAsyncClienteEntity : AsyncPageable<ClienteEntity>
    {
        private readonly IReadOnlyList<ClienteEntity> _items;

        public int GetItemsSize()
        {
            return _items.Count;
        }

        public PageableAsyncClienteEntity(IReadOnlyList<ClienteEntity> items)
        {
            _items = items;
        }

        public override async IAsyncEnumerable<Page<ClienteEntity>> AsPages(string continuationToken = null, int? pageSizeHint = null)
        {
            Mock<Response> responseMock = new Mock<Response>();
            responseMock.SetupGet(r => r.Status).Returns(200);

            Response response = responseMock.Object;

            var page = Page<ClienteEntity>.FromValues(_items, continuationToken, response);
            yield return page;
        }
    }
}