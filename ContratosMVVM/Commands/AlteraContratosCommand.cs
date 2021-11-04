using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels.DialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Commands
{
    public class AlteraContratosCommand : ICommand
    {
        private ClienteDialogViewModel _clienteDialogViewModel;
        private IDialogViewModelFactory _dialogViewModelFactory;
        private IDialogGenerator _dialogGenerator;
        private IDialogsStore _dialogsStore;
        public AlteraContratosCommand(ClienteDialogViewModel clienteDialogViewModel, IServiceProvider serviceProvider)
        {
            _clienteDialogViewModel = clienteDialogViewModel;
            _dialogGenerator = serviceProvider.GetRequiredService<IDialogGenerator>();
            _dialogViewModelFactory = serviceProvider.GetRequiredService<IDialogViewModelFactory>();
            _dialogsStore = serviceProvider.GetRequiredService<IDialogsStore>();
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
