using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels;
using ContratosMVVM.ViewModels.DialogViewModels;
using ContratosMVVM.ViewModels.Factories;
using ContratosMVVM.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContratosMVVM.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices((context, serviços) =>
            {
                serviços.AddSingleton<ViewModelFactory>();
                serviços.AddTransient<MainViewModel>();
                serviços.AddSingleton<HomeViewModel>();

                serviços.AddSingleton<CriaViewModel<HomeViewModel>>(serviços =>
                    serviços.GetRequiredService<HomeViewModel>);

                serviços.AddSingleton<DialogMainViewModel>();
                serviços.AddSingleton<IDialogViewModelFactory, DialogViewModelFactory>();

                serviços.AddTransient<ClienteDialogViewModel>();
                serviços.AddTransient<EditaContratosBaseViewModel>();
                serviços.AddTransient<GeraContasViewModel>();

                serviços.AddSingleton<CriaDialogViewModel<ClienteDialogViewModel>>(serviços =>
                    serviços.GetRequiredService<ClienteDialogViewModel>);
                serviços.AddSingleton<CriaDialogViewModel<EditaContratosBaseViewModel>>(serviços =>
                    serviços.GetRequiredService<EditaContratosBaseViewModel>);
                serviços.AddSingleton<CriaDialogViewModel<GeraContasViewModel>>(serviços =>
                    serviços.GetRequiredService<GeraContasViewModel>);

            });
            return host;
        }
    }
}
