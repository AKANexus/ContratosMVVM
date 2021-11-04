using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContratosMVVM.Generics
{
    public delegate TViewModel CriaDialogViewModel<TViewModel>() where TViewModel : DialogContentViewModelBase;

    public class DialogContentViewModelBase : ViewModelBase
    {
    }
}
