using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Domain;
using ContratosMVVM.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContratosMVVM.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices((context, serviços) =>
            {
                serviços.AddSingleton<ContratoDataService>();
                serviços.AddSingleton<ContratoBaseDataService>();
                serviços.AddSingleton<SetorDataService>();
                serviços.AddSingleton<INIFileService>();
                serviços.AddSingleton<ContratoDocService>();
                serviços.AddSingleton<ObservacaoDataService>()
                    ;
            });
            return host;
        }
    }
}
