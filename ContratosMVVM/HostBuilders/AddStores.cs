using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Services;
using ContratosMVVM.States;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.HostBuilders
{
    public static class AddStoresHostBuilderExtensions
    {
        public static IHostBuilder AddStores(this IHostBuilder host)
        {
            host.ConfigureServices((_, serviços) =>
            {

                serviços.AddSingleton<INavigator, Navigator>();
                serviços.AddSingleton<IDialogsStore, DialogsStore>();
                serviços.AddSingleton<MutexStore>();

                serviços.AddSingleton<ClienteStore>();
            });

            return host;
        }
    }
}
