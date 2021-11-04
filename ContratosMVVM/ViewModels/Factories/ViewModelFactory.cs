using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Generics;
using ContratosMVVM.States;

namespace ContratosMVVM.ViewModels.Factories
{
    public class ViewModelFactory
    {
        private readonly CriaViewModel<HomeViewModel> _createHomeViewModelFactory;
        //private readonly CriaViewModel<ContatosViewModel> _createContatosViewModelFactory;
        //private readonly CriaViewModel<EstoqueViewModel> _createEstoqueViewModelFactory;
        //private readonly CriaViewModel<ComprasViewModel> _createComprasViewModelFactory;
        //private readonly CriaViewModel<LoginViewModel> _createLoginViewModelFactory;
        //private readonly CriaViewModel<VendasViewModel> _createVendasViewModelFactory;

        public ViewModelFactory(
            CriaViewModel<HomeViewModel> createHomeViewModelFactory
            //CriaViewModel<ContatosViewModel> createContatosViewModelFactory,
            //CriaViewModel<EstoqueViewModel> createEstoqueViewModelFactory,
            //CriaViewModel<ComprasViewModel> createComprasViewModelFactory,
            //CriaViewModel<LoginViewModel> createLoginViewModelFactory,
            //CriaViewModel<VendasViewModel> createVendasViewModelFactory
            )
        {
            _createHomeViewModelFactory = createHomeViewModelFactory;
            //_createContatosViewModelFactory = createContatosViewModelFactory;
            //_createEstoqueViewModelFactory = createEstoqueViewModelFactory;
            //_createComprasViewModelFactory = createComprasViewModelFactory;
            //_createLoginViewModelFactory = createLoginViewModelFactory;
            //_createVendasViewModelFactory = createVendasViewModelFactory;
        }

        public ViewModelBase CreateViewModel(TipoView tipoView)
        {
            switch (tipoView)
            {
                //case TipoView.Login:
                //    return _createLoginViewModelFactory();
                case TipoView.Home:
                    return _createHomeViewModelFactory();
                //case TipoView.Contatos:
                //    return _createContatosViewModelFactory();
                //case TipoView.Estoque:
                //    return _createEstoqueViewModelFactory();
                //case TipoView.NotasDeCompra:
                //    return _createComprasViewModelFactory();
                //case TipoView.NotasDeVenda:
                //    return _createVendasViewModelFactory();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
