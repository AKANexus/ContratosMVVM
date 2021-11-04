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
    class EditaSelecionadoCommand : ICommand
    {
        private IDialogGenerator _dialogGenerator;
        private IDialogViewModelFactory _dialogViewModelFactory;
        private IDialogsStore _dialogsStore;
        private ClienteStore _clienteStore;

        public EditaSelecionadoCommand(HomeViewModel homeViewModel, IServiceProvider serviceProvider)
        {
            _clienteStore = serviceProvider.GetRequiredService<ClienteStore>();
            _dialogViewModelFactory = serviceProvider.GetRequiredService<IDialogViewModelFactory>();
            _dialogsStore = serviceProvider.GetRequiredService<IDialogsStore>();
            _dialogGenerator = serviceProvider.GetRequiredService<IDialogGenerator>();
        }


        public bool CanExecute(object? parameter)
        {
            return _clienteStore.Cliente is not null;
        }

        public void Execute(object? parameter)
        {
            _dialogGenerator.ViewModelExibido = _dialogViewModelFactory.CreateDialogContentViewModel(TipoDialog.EditaCliente);
            _dialogsStore.RegisterDialog(_dialogGenerator);
            //_clienteStore.Cliente = null;
        }

        public event EventHandler? CanExecuteChanged;
    }
}
