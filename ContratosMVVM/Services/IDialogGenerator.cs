using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Generics;
using ContratosMVVM.Views.Dialogs;

namespace ContratosMVVM.Services
{
    public enum DialogResult
    {
        Yes,
        No,
        OK,
        Cancel,
        Escape,
        Closed
    }
    public enum TipoDialog
    {
        EditaCliente,
        EditaContratos,
        GeraContasView
    }
    public interface IDialogGenerator
    {
        public int IDSelecionado { get; set; }
        public DialogContentViewModelBase ViewModelExibido { get; set; }
        public void ShowDialog(string windowTitle = "Título Não Fornecido");
        public void Show(string windowTitle = "Título Não Fornecido");

        public string Teste { get; set; }
        public DialogResult Resultado { get; set; }
        public DialogMainView janela { get; set; }
        public event Action DialogClosed;
        public void Close();

    }
}
