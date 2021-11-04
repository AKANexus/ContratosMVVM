using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContratosMVVM.Commands;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.States;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ContratosMVVM.ViewModels.DialogViewModels
{
    public class ClienteDialogViewModel : DialogContentViewModelBase
    {
        private Visibility _painelAberto = Visibility.Visible;
        private CONTRATO _contratoSelecionado = new();
        private ContratoDataService _contratoDataService;
        private ContratoBaseDataService _contratoBaseDataService;
        private ObservacaoDataService _observacaoDataService;
        private ClienteStore _clienteStore;

        public CLIENTE ClienteSelecionado
        {
            get => _clienteStore.Cliente;
            set
            {
                _clienteStore.Cliente = value;
                OnPropertyChanged(nameof(ClienteSelecionado));
            }
        }
        public Visibility PainelAberto
        {
            get => _painelAberto;
            set { _painelAberto = value; OnPropertyChanged(nameof(PainelAberto)); }
        }
        public CONTRATO ContratoSelecionado
        {
            get => _contratoSelecionado;
            set
            {
                if (value is null) return;
                _contratoSelecionado = value;
                OnPropertyChanged(null);

            }
        }



        public ObservableCollection<CONTRATO> Contratos { get; set; } = new();
        public ObservableCollection<CONTRATO_BASE> TiposDeContratoList { get; set; } = new();

        public ICommand Adicionar { get; set; }

        public ICommand SalvaContratoSelecionado { get; set; }

        public decimal ValorUnitário
        {
            get => ContratoSelecionado?.ValorUnitário ?? 0;
            set
            {
                ContratoSelecionado.ValorUnitário = value;
                OnPropertyChanged(nameof(ValorTotalDoContrato));

            }
        }

        public CONTRATO_BASE Nome
        {
            get => ContratoSelecionado.ContratoBase;
            set => ContratoSelecionado.ContratoBaseId = value.Id;
        }

        public int Vigência
        {
            get => ContratoSelecionado?.Vigência ?? 0;
            set => ContratoSelecionado.Vigência = value;
        }

        public decimal ValorTotalDoContrato => ContratoSelecionado?.ValorTotalDoContrato ?? 0;

        public decimal Quantidade
        {
            get => ContratoSelecionado?.Quantidade ?? 0;
            set
            {
                ContratoSelecionado.Quantidade = value;
                OnPropertyChanged(nameof(ValorTotalDoContrato));
            }
        }

        public ICommand GerarContasAReceber { get; set; }

        public ICommand RemoveContratoSelecionado { get; set; }

        public object ValorTotalDosContratos => Contratos?.Sum(x => x.ValorTotalDoContrato) ?? 0;

        public ICommand ImprimeWordDocumento { get; set; }

        public ICommand CarregaArquivoPDF { get; set; }

        public ICommand SaveObservacao { get; set; }

        public ClienteDialogViewModel(HomeViewModel homeViewModel, IServiceProvider serviceProvider)
        {
            Adicionar = new AdicionarCommand(this);
            _clienteStore = serviceProvider.GetRequiredService<ClienteStore>();
            SalvaContratoSelecionado = new SalvaContratoSelecionadoCommand(this, serviceProvider, (x) => MessageBox.Show(x.Message));
            RemoveContratoSelecionado =
                new RemoveContratoSelecionadoCommand(this, serviceProvider, (x) => MessageBox.Show(x.Message));
            ImprimeWordDocumento = new ImprimeWordDocCommand(this, serviceProvider);
            GerarContasAReceber = new AbreJanelaDeContasAReceber(serviceProvider);
            CarregaArquivoPDF = new CarregaArquivoPDF(this, serviceProvider, (x) => MessageBox.Show(x.Message));
            SaveObservacao = new SaveObservacaoCommand(this, serviceProvider, (x) => MessageBox.Show(x.Message));
            _contratoBaseDataService = serviceProvider.GetRequiredService<ContratoBaseDataService>();
            _contratoDataService = serviceProvider.GetRequiredService<ContratoDataService>();
            _observacaoDataService = serviceProvider.GetRequiredService<ObservacaoDataService>();
            ClienteSelecionado = homeViewModel.ClienteSelecionado;
            OnPropertyChanged(nameof(ClienteSelecionado));
            OnPropertyChanged(nameof(ClienteSelecionado.Contratos));

            PreencheInformações();
        }

        public void AtualizaView()
        {
            PreencheInformações();
        }
        private async Task PreencheInformações()
        {
            TiposDeContratoList.Clear();
            Contratos.Clear();
            foreach (var contratoBase in await _contratoBaseDataService.GetAllAsNoTracking())
            {
                TiposDeContratoList.Add(contratoBase);
            }

            foreach (var contrato in (await _contratoDataService.GetAllAsNoTracking()).Where(x => x.FirebirdIDCliente == ClienteSelecionado.IDFirebird))
            {
                Contratos.Add(contrato);
            }
            OnPropertyChanged(nameof(ValorTotalDosContratos));
            try
            {
                ClienteSelecionado.Observacao =
                    await _observacaoDataService.GetAsNoTrackingByFirebirdId(ClienteSelecionado.IDFirebird);
                if (ClienteSelecionado.Observacao is null) ClienteSelecionado.Observacao = new();
                OnPropertyChanged(nameof(ClienteSelecionado.Observacao.Texto));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class SaveObservacaoCommand : AsyncCommandBase
    {
        private readonly ClienteDialogViewModel _clienteDialogViewModel;
        private readonly ObservacaoDataService _observacaoDataService;

        public SaveObservacaoCommand(ClienteDialogViewModel clienteDialogViewModel, IServiceProvider serviceProvider, Action<Exception> onException) : base(onException)
        {
            _clienteDialogViewModel = clienteDialogViewModel;
            _observacaoDataService = serviceProvider.GetRequiredService<ObservacaoDataService>();
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            if (_clienteDialogViewModel.ClienteSelecionado.Observacao.FirebirdId == 0)
            {
                _clienteDialogViewModel.ClienteSelecionado.Observacao.FirebirdId =
                    _clienteDialogViewModel.ClienteSelecionado.IDFirebird;
                await _observacaoDataService.Create(_clienteDialogViewModel.ClienteSelecionado.Observacao);
            }
            else
            {
                await _observacaoDataService.Update(_clienteDialogViewModel.ClienteSelecionado.Observacao.Id,
                    _clienteDialogViewModel.ClienteSelecionado.Observacao);

            }

            MessageBox.Show("Observações salvas com sucesso!");
        }
    }

    public class CarregaArquivoPDF : AsyncCommandBase
    {
        private readonly ClienteDialogViewModel _clienteDialogViewModel;
        private ContratoDataService _contratoDataService;
        public CarregaArquivoPDF(ClienteDialogViewModel clienteDialogViewModel, IServiceProvider serviceProvider, Action<Exception> onException) : base(onException)
        {
            _clienteDialogViewModel = clienteDialogViewModel;
            _contratoDataService = serviceProvider.GetRequiredService<ContratoDataService>();
            _clienteDialogViewModel.PropertyChanged += _clienteDialogViewModel_PropertyChanged;
        }

        private void _clienteDialogViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public override bool CanExecute(object? parameter)
        {
            return _clienteDialogViewModel.Contratos.Count > 0;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arquivos PDF (.pdf)|*.pdf";
            ofd.DefaultExt = ".pdf";
            if (ofd.ShowDialog() is true)
            {
                Directory.CreateDirectory(@"Contratos\Assinados");
                string razãoSocialDoCliente = _clienteDialogViewModel.ClienteSelecionado.RazãoSocial;
                var arquivos = Directory.GetFiles(@"Contratos\Assinados",
                    $@"{razãoSocialDoCliente.Replace("\\", string.Empty).Replace("/", string.Empty)}-{DateTime.Today:dd-MM-yy}*");
                if (arquivos.Length > 0)
                {
                    File.Copy(ofd.FileName, $@"Contratos\Assinados\{razãoSocialDoCliente.Replace("\\", string.Empty).Replace("/", string.Empty)}-{DateTime.Today:dd-MM-yy}_{arquivos.Length:0000}.pdf");
                    foreach (CONTRATO contrato in await _contratoDataService.GetAllAsNoTrackingByCliente(_clienteDialogViewModel.ClienteSelecionado))
                    {
                        contrato.ContratoPDF = $@"Contratos\Assinados\{razãoSocialDoCliente.Replace("\\", string.Empty).Replace("/", string.Empty)}-{DateTime.Today:dd-MM-yy}_{arquivos.Length:0000}.pdf";
                        await _contratoDataService.Update(contrato.Id, contrato);
                    }
                    MessageBox.Show("Contrato assinado atualizado com sucesso!");

                }
                else
                {
                    File.Copy(ofd.FileName,
                        $@"Contratos\Assinados\{_clienteDialogViewModel.ClienteSelecionado.RazãoSocial.Replace("\\", string.Empty).Replace("/", string.Empty)}-{DateTime.Today:dd-MM-yy}.pdf");
                    foreach (CONTRATO contrato in await _contratoDataService.GetAllAsNoTrackingByCliente(
                        _clienteDialogViewModel.ClienteSelecionado))
                    {
                        contrato.ContratoPDF = $@"Contratos\Assinados\{_clienteDialogViewModel.ClienteSelecionado.RazãoSocial.Replace("\\", string.Empty).Replace("*", string.Empty)}-{DateTime.Today:dd-MM-yy}.pdf";
                        await _contratoDataService.Update(contrato.Id, contrato);
                    }
                    MessageBox.Show("Contrato assinado salvo com sucesso!");
                }


            }
            else return;
        }

        public override event EventHandler? CanExecuteChanged;
    }
}
