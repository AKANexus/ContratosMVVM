using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels.DialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Commands
{
    public class EditaContratobaseSelecionado : AsyncCommandBase
    {
        private readonly EditaContratosBaseViewModel _editaContratosBaseViewModel;
        private ContratoBaseDataService _dataService;

        public EditaContratobaseSelecionado(EditaContratosBaseViewModel editaContratosBaseViewModel, IServiceProvider serviceProvider, Action<Exception> onExAction) : base(onExAction)
        {
            _dataService = serviceProvider.GetRequiredService<ContratoBaseDataService>();
            _editaContratosBaseViewModel = editaContratosBaseViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            //await _dataService.Get(_editaContratosBaseViewModel.ContratoBaseSelecionadoDGV.Id);
            return;
        }

        public event EventHandler CanExecuteChanged;
    }
}
