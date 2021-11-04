using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContratosMVVM.Domain;
using ContratosMVVM.ViewModels.DialogViewModels;

namespace ContratosMVVM.Commands
{
    public class AdicionaNovoContratoCommand : ICommand
    {
        private readonly EditaContratosBaseViewModel _editaContratosBaseViewModel;

        public AdicionaNovoContratoCommand(EditaContratosBaseViewModel editaContratosBaseViewModel)
        {
            _editaContratosBaseViewModel = editaContratosBaseViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _editaContratosBaseViewModel.ContratoBaseSelecionado = new CONTRATO_BASE();
        }

        public event EventHandler CanExecuteChanged;
    }
}
