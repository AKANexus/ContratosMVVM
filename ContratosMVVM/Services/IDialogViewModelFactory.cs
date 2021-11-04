using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Generics;

namespace ContratosMVVM.Services
{
    public interface IDialogViewModelFactory
    {
        DialogContentViewModelBase CreateDialogContentViewModel(TipoDialog tipoView);

    }
}
