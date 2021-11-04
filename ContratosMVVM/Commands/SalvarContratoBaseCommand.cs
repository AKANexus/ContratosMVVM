using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels.DialogViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ContratosMVVM.Commands
{
    public class SalvarContratoBaseCommand : AsyncCommandBase
    {
        private readonly EditaContratosBaseViewModel _editaContratosBaseViewModel;
        private readonly ContratoBaseDataService _contratoBaseDataService;
        private readonly SetorDataService _setorDataService;

        public SalvarContratoBaseCommand(EditaContratosBaseViewModel editaContratosBaseViewModel, IServiceProvider serviceProvider, Action<Exception> onException) : base(onException)
        {
            _contratoBaseDataService = serviceProvider.GetRequiredService<ContratoBaseDataService>();
            _setorDataService = serviceProvider.GetRequiredService<SetorDataService>();
            _editaContratosBaseViewModel = editaContratosBaseViewModel;
            //_editaContratosBaseViewModel.PropertyChanged += _editaContratosBaseViewModel_PropertyChanged;
            _editaContratosBaseViewModel.ContratoBaseSelecionado.PropertyChanged += ContratoBaseSelecionado_PropertyChanged;
        }

        private void ContratoBaseSelecionado_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public override bool CanExecute(object parameter)
        {
            if (string.IsNullOrWhiteSpace(_editaContratosBaseViewModel.ContratoBaseSelecionado.Nome)) return false;
            if (string.IsNullOrWhiteSpace(_editaContratosBaseViewModel.ContratoBaseSelecionado.Descrição)) return false;
            return true;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            if (_editaContratosBaseViewModel.ContratoBaseSelecionado.Id == 0)
            {
                try
                {
                    var result = await _contratoBaseDataService.Create(_editaContratosBaseViewModel.ContratoBaseSelecionado);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                if (MessageBox.Show("Deseja sobrescrever o contrato selcionado?", "Editar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    await _contratoBaseDataService.Update(_editaContratosBaseViewModel.ContratoBaseSelecionado.Id, _editaContratosBaseViewModel.ContratoBaseSelecionado);
            }

            _editaContratosBaseViewModel.ContratoBaseSelecionado = new CONTRATO_BASE();
            _editaContratosBaseViewModel.AtualizaListagem();
        }

        public override event EventHandler CanExecuteChanged;
    }
}
