using System;
using System.Diagnostics;
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
        private string _mes;
        private string _ano;
        private string _diaVencimento;

        public GeraContasViewModel(IServiceProvider serviceProvider)
        {
            _clienteStore = serviceProvider.GetRequiredService<ClienteStore>();
            GerarContasAReceber = new GerarContasAReceberBlingCommand(this, serviceProvider, _clienteStore.Cliente, x => MessageBox.Show(x.Message));
        }

        public string NomeCliente => _clienteStore.Cliente?.RazãoSocial;

        public string Mes
        {
            get => _mes;
            set { _mes = value; OnPropertyChanged(nameof(Mes)); }
        }

        public string Ano
        {
            get => _ano;
            set { _ano = value; OnPropertyChanged(nameof(Ano)); }
        }

        public string DiaVencimento
        {
            get => _diaVencimento;
            set { _diaVencimento = value; OnPropertyChanged(nameof(DiaVencimento)); }
        }

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
            geraContasViewModel.PropertyChanged += GeraContasViewModel_PropertyChanged;
        }

        private void GeraContasViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public override event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            return int.TryParse(string.IsNullOrWhiteSpace(_geraContasViewModel.Mes) ? "0" : _geraContasViewModel.Mes, out _mes) &&
                   int.TryParse(_geraContasViewModel.Ano, out _ano) &&
                   int.TryParse(_geraContasViewModel.DiaVencimento, out _diaVenc);
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            if (_mes == 0)
            {
                for (int i = DateTime.Today.Month; i < 13; i++)
                {
                    await _api.POSTContaReceber(progressIndicator, _clienteStoreCliente, new(_ano, i, _diaVenc));
                }
                MessageBox.Show("ContaReceber do ano gerada");

            }
            else
            {
                await _api.POSTContaReceber(progressIndicator, _clienteStoreCliente, new(_ano, _mes, _diaVenc));
                MessageBox.Show("ContaReceber gerada");
            }
        }
    }
}
