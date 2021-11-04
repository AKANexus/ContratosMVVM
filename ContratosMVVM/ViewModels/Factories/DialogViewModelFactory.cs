using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.States;
using ContratosMVVM.ViewModels.DialogViewModels;

namespace ContratosMVVM.ViewModels.Factories
{
    public class DialogViewModelFactory : IDialogViewModelFactory
    {
        private readonly CriaDialogViewModel<ClienteDialogViewModel> _createClienteDialogViewModel;
        private readonly CriaDialogViewModel<EditaContratosBaseViewModel> _createContratosDialogViewModel;
        private readonly CriaDialogViewModel<GeraContasViewModel> _createGeraContasViewModel;
        public DialogViewModelFactory(CriaDialogViewModel<ClienteDialogViewModel> createClienteDialogViewModel,
            CriaDialogViewModel<EditaContratosBaseViewModel> createContratosDialogViewModel, 
            CriaDialogViewModel<GeraContasViewModel> createGeraContasViewModel)
        {
            _createClienteDialogViewModel = createClienteDialogViewModel;
            _createContratosDialogViewModel = createContratosDialogViewModel;
            _createGeraContasViewModel = createGeraContasViewModel;
        }

        public DialogContentViewModelBase CreateDialogContentViewModel(TipoDialog tipoView)
        {

            return tipoView switch
            {
                TipoDialog.EditaContratos => _createContratosDialogViewModel(),
                TipoDialog.EditaCliente => _createClienteDialogViewModel(),
                TipoDialog.GeraContasView => _createGeraContasViewModel(),
                _ => throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType")
            };
        }
    }
}
