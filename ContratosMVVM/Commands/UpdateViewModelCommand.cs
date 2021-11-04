using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContratosMVVM.States;
using ContratosMVVM.ViewModels.Factories;

namespace ContratosMVVM.Commands
{
    public class UpdateViewModelAtualCommand : ICommand
    {
        private readonly INavigator _navigator;
        private readonly ViewModelFactory _viewModelFactory;

        public UpdateViewModelAtualCommand(INavigator navigator, ViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is TipoView)
            {
                TipoView tipoView = (TipoView)parameter;

                _navigator.ViewModelAtual = _viewModelFactory.CreateViewModel(tipoView);
            }
        }
    }
}
