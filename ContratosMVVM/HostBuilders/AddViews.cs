using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels;
using ContratosMVVM.Views;
using ContratosMVVM.Views.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContratosMVVM.HostBuilders
{
    public static class AddViewsHostBuilderExtensions
    {
        public static IHostBuilder AddViews(this IHostBuilder host)
        {
            host.ConfigureServices((context, serviços) => {
                serviços.AddSingleton<MainView>(CriaMainView);
                serviços.AddTransient<DialogMainView>();
                serviços.AddScoped<IDialogGenerator, DialogGenerator>();
            });
            return host;
        }
        private static MainView CriaMainView(IServiceProvider serviços)
        {
            return new(serviços.GetRequiredService<MainViewModel>());
        }
    }
}
