using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContratosMVVM.ViewModels.DialogViewModels;

namespace ContratosMVVM.Commands
{
    public class AdicionarCommand : ICommand
    {
        private readonly ClienteDialogViewModel _clienteDialogViewModel;

        public AdicionarCommand(ClienteDialogViewModel clienteDialogViewModel)
        {
            _clienteDialogViewModel = clienteDialogViewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _clienteDialogViewModel.PainelAberto = _clienteDialogViewModel.PainelAberto switch
            {
                Visibility.Collapsed => Visibility.Visible,
                Visibility.Visible => Visibility.Collapsed,
                _ => _clienteDialogViewModel.PainelAberto
            };
        }
    }
}

