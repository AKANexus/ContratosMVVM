using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContratosMVVM.Services
{
    public interface IDialogsStore
    {
        public List<IDialogGenerator> OpenDialogs { get; set; }

        public int RegisterDialog(IDialogGenerator dialog, string windowTitle = "Título Não Fornecido", bool IsModal = true);
        public int RegisterDialog(TipoDialog dialog, string windowTitle = "Título Não Fornecido", bool IsModal = true);
        public void CloseDialog(DialogResult dialogResult);
    }
}
