using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContratosMVVM.Commands;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.States;
using ContratosMVVM.ViewModels.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelFactory _viewModelFactory;
        private readonly INavigator _navigator;



        public ViewModelBase ViewModelAtual => _navigator.ViewModelAtual;
        //public Visibility MenuStripVisibility { get; set; } = Visibility.Collapsed;

        public MainViewModel(INavigator navigator, ViewModelFactory viewModelFactory, IServiceProvider escopo)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;

            UpdateViewModelAtual = new UpdateViewModelAtualCommand(navigator, _viewModelFactory);
            UpdateViewModelAtual.Execute(TipoView.Home);

        }

        //private void _navigator_StateChanged()
        //{
        //    if (ViewModelAtual is LoginViewModel)
        //    {
        //        MenuStripVisibility = Visibility.Collapsed;
        //        OnPropertyChanged(nameof(MenuStripVisibility));
        //    }
        //    else
        //    {
        //        MenuStripVisibility = Visibility.Visible;
        //        OnPropertyChanged(nameof(MenuStripVisibility));
        //    }
        //    OnPropertyChanged(nameof(ViewModelAtual));
        //}

        public ICommand UpdateViewModelAtual { get; set; }
        //public ICommand MuttleyFacaAlgumaCoisa { get; set; }

    }

}