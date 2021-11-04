using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Generics;

namespace ContratosMVVM.States
{
    public enum TipoView
    {
        Home
    }


    public interface INavigator
    {
        public ViewModelBase ViewModelAtual { get; set; }
        event Action StateChanged;
        public int IDSelecionado { get; set; }
    }
}
