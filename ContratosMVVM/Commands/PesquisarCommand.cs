using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.States;
using ContratosMVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Commands
{
    public class PesquisarCommand : AsyncCommandBase
    {
        private HomeViewModel _homeViewModel;
        private ContratoDataService _contratoDataService;
        private ClienteStore _clienteStore;
        public PesquisarCommand(HomeViewModel homeViewModel, IServiceProvider serviceProvider,
            Action<Exception> onException) : base(onException)
        {
            _homeViewModel = homeViewModel;
            _homeViewModel.PropertyChanged += _homeViewModel_PropertyChanged;
            _clienteStore = serviceProvider.GetRequiredService<ClienteStore>();
            _contratoDataService = serviceProvider.GetRequiredService<ContratoDataService>();
        }

        private void _homeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public override bool CanExecute(object parameter)
        {
            return _homeViewModel.CampoDePesquisa?.Length > 3;
        }

        public override event EventHandler CanExecuteChanged;

        protected override async Task ExecuteAsync(object parameter)
        {
            _homeViewModel.ClientesList.Clear();
            _clienteStore.Cliente = null;
            foreach (var cliente in _homeViewModel.ClientesListFull.Where(x=>x.RazãoSocial.Contains(_homeViewModel.CampoDePesquisa)))
            {
                cliente.Contratos = await _contratoDataService.GetAllAsNoTrackingByCliente(cliente);
                _homeViewModel.ClientesList.Add(cliente);
            }
            //_homeViewModel.ClientesList.Add(new()
            //{
            //    RazãoSocial = "Teste Armando de Salles",
            //    CNPJCPF = "102.499.043-57"
            //});
        }
    }
}
