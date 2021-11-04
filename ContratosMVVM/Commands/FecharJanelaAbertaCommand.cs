using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContratosMVVM.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Commands
{
    public class FecharJanelaAbertaCommand : ICommand
    {
        private IDialogsStore _dialogsStore;
        public FecharJanelaAbertaCommand(IServiceProvider serviceProvider)
        {
            _dialogsStore = serviceProvider.GetRequiredService<IDialogsStore>();
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _dialogsStore.CloseDialog(DialogResult.Closed);
        }

        public event EventHandler? CanExecuteChanged;
    }
}
