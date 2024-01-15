
using Api.Application;
using Api.Application.Interfaces;
using Api.Domain;
using Api.Domain.Interfaces;
using Api.Infra;
using Autofac;

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
        }
    }
}
