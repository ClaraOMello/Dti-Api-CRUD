
using Api.Application;
using Api.Application.Interfaces;
using Api.Domain;
using Api.Domain.Interfaces;
using Api.Infra;
using Api.Infra.Converters;
using Api.Infra.TablesEntities;
using Api.Models;
using Api.Shared;
using Autofac;
using Azure.Data.Tables;

namespace Api
{
    public class IocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProcessarClienteAppService>().As<IProcessarClienteAppService>();
            builder.RegisterType<ProcessarClienteService>().As<IProcessarClienteService>();
            builder.RegisterType<ProcessarFaixaEtaria>().As<IProcessarFaixaEtaria>();
            builder.RegisterType<ProcessarClienteAzureTable>().As<IProcessarClienteRepository>();
            builder.RegisterType<ClienteConverter>().As<ITwoWayConverter<Cliente, ClienteEntity>>();
            builder.RegisterType<ClienteConverter>().As<ITwoWayConverter<Cliente, ClienteEntity>>();
            builder.Register(c =>
            {
                return new TableServiceClient("UseDevelopmentStorage=true");
            }).As<TableServiceClient>().SingleInstance();
        }
    }
}
