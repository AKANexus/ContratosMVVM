using System;
using System.Windows;
using System.Windows.Input;
using ContratosMVVM.Commands;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using ContratosMVVM.States;
using ContratosMVVM.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.ViewModels.DialogViewModels
{
    public class GeraContasViewModel : DialogContentViewModelBase
    {
        private readonly ClienteStore _clienteStore;
        public GeraContasViewModel(IServiceProvider serviceProvider)
        {
            _clienteStore = serviceProvider.GetRequiredService<ClienteStore>();
            GerarContasAReceber = new GerarContasAReceberCommand(this, serviceProvider, _clienteStore.Cliente, (x)=>MessageBox.Show(x.Message));
        }

        public string NomeCliente => _clienteStore.Cliente?.RazãoSocial;

        public string Mes { get; set; }

        public string Ano { get; set; }

        public string DiaVencimento { get; set; }

        public ICommand GerarContasAReceber { get; set; }
    }
}
