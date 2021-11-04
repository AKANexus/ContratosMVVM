using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels.DialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Commands
{
    public class SalvaContratoSelecionadoCommand : AsyncCommandBase
    {
        private readonly ClienteDialogViewModel _clienteDialogViewModel;
        private ContratoDataService _genericDataService;

        public SalvaContratoSelecionadoCommand(ClienteDialogViewModel clienteDialogViewModel, IServiceProvider serviceProvider, Action<Exception> onException) : base(onException)
        {
            _clienteDialogViewModel = clienteDialogViewModel;
            _genericDataService = serviceProvider.GetRequiredService<ContratoDataService>();
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            _clienteDialogViewModel.ContratoSelecionado.FirebirdIDCliente =
                _clienteDialogViewModel.ClienteSelecionado.IDFirebird;
            if (_clienteDialogViewModel.ContratoSelecionado.Id == 0)
            {
                await _genericDataService.Create(_clienteDialogViewModel.ContratoSelecionado);
            }
            else
            {
                await _genericDataService.Update(_clienteDialogViewModel.ContratoSelecionado.Id,
                    _clienteDialogViewModel.ContratoSelecionado);
            }

            _clienteDialogViewModel.ContratoSelecionado = new();
            _clienteDialogViewModel.AtualizaView();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
