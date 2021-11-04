using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContratosMVVM.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Commands
{
    public class EditarContratosCommand : ICommand
    {
        private IDialogGenerator _dialogGenerator;
        private IDialogViewModelFactory _dialogViewModelFactory;
        private IDialogsStore _dialogsStore;
        public EditarContratosCommand(IServiceProvider serviceProvider)
        {
            _dialogGenerator = serviceProvider.GetRequiredService<IDialogGenerator>();
            _dialogViewModelFactory = serviceProvider.GetRequiredService<IDialogViewModelFactory>();
            _dialogsStore = serviceProvider.GetRequiredService<IDialogsStore>();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _dialogGenerator.ViewModelExibido =
                _dialogViewModelFactory.CreateDialogContentViewModel(TipoDialog.EditaContratos);
            _dialogsStore.RegisterDialog(_dialogGenerator);
        }

        public event EventHandler CanExecuteChanged;
    }
}
