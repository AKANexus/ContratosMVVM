using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Services
{
    public class DialogsStore : IDialogsStore
    {
        private readonly IDialogGenerator _dialogGenerator;
        private readonly IDialogViewModelFactory _dialogVMFactory;

        public DialogsStore(IServiceProvider serviceProvider)
        {
            _dialogGenerator = serviceProvider.GetRequiredService<IDialogGenerator>();
            _dialogVMFactory = serviceProvider.GetRequiredService<IDialogViewModelFactory>();
        }

        public List<IDialogGenerator> OpenDialogs { get; set; } = new();

        //Depois de 16 horas, é que eu consegui fazer isso funcionar. Não mexa nesse código
        //a não ser que eu tenha explicitamente pedido pra você mexer nele.

        public void CloseDialog(DialogResult dialogResult)
        {
            IDialogGenerator lastDialogGenerator = OpenDialogs[OpenDialogs.Count - 1];
            lastDialogGenerator.Resultado = dialogResult;
            //lastDialogGenerator.DialogClosed = null;
            lastDialogGenerator.Close();
        }

        public int RegisterDialog(TipoDialog dialog, string windowTitle = "Título Não Fornecido", bool IsModal = true)
        {
            _dialogGenerator.ViewModelExibido = _dialogVMFactory.CreateDialogContentViewModel(dialog);

            OpenDialogs.Add(_dialogGenerator);
            _dialogGenerator.DialogClosed += Dialog_DialogClosed;
            if (IsModal) _dialogGenerator.ShowDialog(windowTitle);
            else _dialogGenerator.Show(windowTitle);
            return OpenDialogs.Count - 1;
        }

        public int RegisterDialog(IDialogGenerator dialog, string windowTitle = "Título Não Fornecido", bool IsModal = true)
        {
            OpenDialogs.Add(dialog);
            dialog.DialogClosed += Dialog_DialogClosed;
            if (IsModal) dialog.ShowDialog(windowTitle);
            else dialog.Show(windowTitle);
            return OpenDialogs.Count - 1;
        }

        private void Dialog_DialogClosed()
        {
            OpenDialogs.RemoveAt(OpenDialogs.Count - 1);
        }
    }
}
