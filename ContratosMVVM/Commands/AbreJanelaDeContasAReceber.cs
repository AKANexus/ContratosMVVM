using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContratosMVVM.Annotations;
using ContratosMVVM.Domain;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels;
using ContratosMVVM.ViewModels.DialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Commands
{
    public class AbreJanelaDeContasAReceber : ICommand
    {
        private readonly IDialogsStore _dialogsStore;
        private IDialogViewModelFactory _dialogViewModelFactory;
        private IDialogGenerator _dialogGenerator;
        public AbreJanelaDeContasAReceber(IServiceProvider serviceProvider)
        {
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
            _dialogGenerator.ViewModelExibido =
                _dialogViewModelFactory.CreateDialogContentViewModel(TipoDialog.GeraContasView);
            _dialogsStore.RegisterDialog(_dialogGenerator);
        }

        public event EventHandler? CanExecuteChanged;
    }
}
