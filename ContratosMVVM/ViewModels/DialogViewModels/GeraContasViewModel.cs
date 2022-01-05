using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContratosMVVM.Auxiliares;
using ContratosMVVM.Commands;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
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
            GerarContasAReceber = new GerarContasAReceberBlingCommand(this, serviceProvider, _clienteStore.Cliente, (x)=>MessageBox.Show(x.Message));
        }

        public string NomeCliente => _clienteStore.Cliente?.RazãoSocial;

        public string Mes { get; set; }

        public string Ano { get; set; }

        public string DiaVencimento { get; set; }

        public ICommand GerarContasAReceber { get; set; }
    }

    public class GerarContasAReceberBlingCommand : AsyncCommandBase
    {
        private readonly GeraContasViewModel _geraContasViewModel;
        private readonly CLIENTE _clienteStoreCliente;
        private readonly BlingAPIService _api;
        private int _mes, _ano, _diaVenc;
        private readonly Progress<ProgressReport> progressIndicator = new();

        public GerarContasAReceberBlingCommand(GeraContasViewModel geraContasViewModel, IServiceProvider serviceProvider, CLIENTE clienteStoreCliente, Action<Exception> func) : base(func)
        {
            _geraContasViewModel = geraContasViewModel;
            _clienteStoreCliente = clienteStoreCliente;
            _api = serviceProvider.GetRequiredService<BlingAPIService>();
        }

        public override event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            return int.TryParse(_geraContasViewModel.Mes, out _mes) &&
                   int.TryParse(_geraContasViewModel.Ano, out _ano) &&
                   int.TryParse(_geraContasViewModel.DiaVencimento, out _diaVenc);
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            if (_mes == 0)
            {

            }
            else
            {
                await _api.POSTContaReceber(progressIndicator, _clienteStoreCliente, new(_ano, _mes, _diaVenc));
                MessageBox.Show("ContaReceber gerada");
            }
        }
    }
}
