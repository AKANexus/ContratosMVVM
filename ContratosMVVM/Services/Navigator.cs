using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Generics;
using ContratosMVVM.States;

namespace ContratosMVVM.Services
{
    public class Navigator : INavigator
    {
        private ViewModelBase _viewModelAtual;

        public ViewModelBase ViewModelAtual
        {
            get => _viewModelAtual;
            set
            {
                _viewModelAtual = value;
                StateChanged?.Invoke();
            }
        }

        private int idSelecionado;

        public int IDSelecionado
        {
            get => idSelecionado;
            set => idSelecionado = value;
        }


        public event Action StateChanged;
    }
}
