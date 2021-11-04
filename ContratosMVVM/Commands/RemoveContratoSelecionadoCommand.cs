using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels.DialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Commands
{
    public class RemoveContratoSelecionadoCommand : AsyncCommandBase
    {
        private readonly ClienteDialogViewModel _clienteDialogViewModel;
        private readonly ContratoDataService _contratoDataService;

        public RemoveContratoSelecionadoCommand(ClienteDialogViewModel clienteDialogViewModel, IServiceProvider serviceProvider, Action<Exception> func) : base(func)
        {
            _clienteDialogViewModel = clienteDialogViewModel;
            _contratoDataService = serviceProvider.GetRequiredService<ContratoDataService>();
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _contratoDataService.Delete(_clienteDialogViewModel.ContratoSelecionado.Id);
            _clienteDialogViewModel.ContratoSelecionado = new();
            _clienteDialogViewModel.AtualizaView();
        }
    }
}
