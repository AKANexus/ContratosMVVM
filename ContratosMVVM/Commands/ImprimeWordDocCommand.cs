using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels.DialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Commands
{
    public class ImprimeWordDocCommand : ICommand
    {
        private readonly ClienteDialogViewModel _clienteDialogViewModel;
        private readonly ContratoDocService _contratoDocService;
        public ImprimeWordDocCommand(ClienteDialogViewModel clienteDialogViewModel, IServiceProvider serviceProvider)
        {
            _clienteDialogViewModel = clienteDialogViewModel;
            _contratoDocService = serviceProvider.GetRequiredService<ContratoDocService>();
            clienteDialogViewModel.PropertyChanged += _clienteDialogViewModel_PropertyChanged;
        }

        private void _clienteDialogViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public bool CanExecute(object? parameter)
        {
            return _clienteDialogViewModel.Contratos.Count > 0;
        }

        public void Execute(object? parameter)
        {
            _contratoDocService.IniciaNovoArquivo(_clienteDialogViewModel.ClienteSelecionado, _clienteDialogViewModel.Contratos.ToList());
            try
            {
                _contratoDocService.FazAPorraToda();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            MessageBox.Show("Contrato gerado com sucesso! Verifique na pasta \"Contratos\"");
        }

        public event EventHandler? CanExecuteChanged;
    }
}
