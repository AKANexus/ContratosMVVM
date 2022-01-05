using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContratosMVVM.Auxiliares;
using ContratosMVVM.Commands;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using ContratosMVVM.Methods;
using ContratosMVVM.Services;
using ContratosMVVM.States;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly DataTable _dataTable = new();
        private ClienteStore _clienteStore;
        private ContratoDataService _contratoDataService;
        private string _campoDePesquisa;
        private INIFileService _iniFileService;
        private readonly ContatoDataService _clienteDataService;
        private readonly Progress<ProgressReport> progressIndicator = new();
        private Visibility _canvasVisibility = Visibility.Collapsed;
        private bool _hitTestVisible = true;
        private string _tabelaTextBlock;
        private string _entradaTextBlock;

        public ICommand Pesquisar { get; set; }
        public ICommand GerarMensalidades { get; set; }
        public ICommand EditaSelecionado { get; set; }

        public ObservableCollection<CLIENTE> ClientesList { get; set; } = new();
        public List<CLIENTE> ClientesListFull { get; set; } = new();

        public string CampoDePesquisa
        {
            get => _campoDePesquisa;
            set { _campoDePesquisa = value; OnPropertyChanged(nameof(CampoDePesquisa)); }
        }

        public List<string> FiltrosDePesquisa { get; set; } = new();

        public CLIENTE ClienteSelecionado
        {
            get => _clienteStore?.Cliente;
            set { _clienteStore.Cliente = value; OnPropertyChanged(nameof(ClienteSelecionado)); }
        }

        public ICommand EditarContratos { get; set; }
        public ICommand VisualizaPDF { get; set; }
        public ICommand SincronizarComBling { get; set; }

        public string TABELATextBlock
        {
            get => _tabelaTextBlock;
            set { _tabelaTextBlock = value; OnPropertyChanged(nameof(TABELATextBlock)); }
        }

        public string ENTRADATextBlock
        {
            get => _entradaTextBlock;
            set { _entradaTextBlock = value; OnPropertyChanged(nameof(ENTRADATextBlock)); }
        }

        public Visibility CanvasVisibility
        {
            get => _canvasVisibility;
            set { _canvasVisibility = value; OnPropertyChanged(nameof(CanvasVisibility)); }
        }

        public bool HitTestVisible
        {
            get => _hitTestVisible;
            set { _hitTestVisible = value; OnPropertyChanged(nameof(HitTestVisible)); }
        }

        public HomeViewModel(IServiceProvider serviceProvider)
        {
            Pesquisar = new PesquisarCommand(this, serviceProvider, (x) => MessageBox.Show(x.Message));
            EditaSelecionado = new EditaSelecionadoCommand(this, serviceProvider);
            EditarContratos = new EditarContratosCommand(serviceProvider);
            VisualizaPDF = new VisualizaPDVCommand(this, serviceProvider, (x) => MessageBox.Show(x.Message));
            SincronizarComBling = new SincronizarComBlingCommand(this, serviceProvider, progressIndicator, x => MessageBox.Show(x.Message));
            GerarMensalidades = new ExibeDiálogoGeraMensalidade(this, serviceProvider);
            _clienteStore = serviceProvider.GetRequiredService<ClienteStore>();
            _contratoDataService = serviceProvider.GetRequiredService<ContratoDataService>();
            _iniFileService = serviceProvider.GetRequiredService<INIFileService>();
            _clienteDataService = serviceProvider.GetRequiredService<ContatoDataService>();
            progressIndicator.ProgressChanged += ProgressIndicator_ProgressChanged;
            PreencheClientesFull();
        }

        private void ProgressIndicator_ProgressChanged(object sender, ProgressReport e)
        {
            TABELATextBlock = e.TABELA;
            ENTRADATextBlock = e.ENTRADA;
        }

        private async Task PreencheClientesFull()
        {

            foreach (CLIENTE cliente in await _clienteDataService.GetAllAsNoTracking())
            {
                ClientesListFull.Add(cliente);
            }
            //string firebirdLocation = _iniFileService.Get(INIConfig.BaseClipp);
            //using var fbConnection =
            //    new FbConnection(
            //        $@"initial catalog={firebirdLocation};data source=192.168.10.250;user id=SYSDBA;password=masterke;encoding=WIN1252;charset=utf8");
            //using var fbCommand = new FbCommand { Connection = fbConnection, CommandType = CommandType.Text, CommandText = "SELECT * FROM V_CLIENTES" };
            //using var fbDataAdapter = new FbDataAdapter(fbCommand);
            //try
            //{
            //    fbConnection.Open();
            //    fbDataAdapter.Fill(_dataTable);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}



            //foreach (DataRow dataRow in _dataTable.Rows)
            //{
            //    CLIENTE cli = new();
            //    try
            //    {
            //        string cnpjcpf = null;
            //        if (dataRow["CPF"] != DBNull.Value)
            //            cnpjcpf = (string)dataRow["CPF"];
            //        if (dataRow["CNPJ"] != DBNull.Value)
            //            cnpjcpf = (string)dataRow["CNPJ"];

            //        cli.CNPJCPF = cnpjcpf;
            //        string ddd = (string)(dataRow["DDD_RESID"] == DBNull.Value ? null : dataRow["DDD_RESID"]);
            //        string telefone = (string)(dataRow["FONE_RESID"] == DBNull.Value ? null : dataRow["FONE_RESID"]);

            //        cli.Telefone = $"{ddd}-{telefone}";
            //        cli.RazãoSocial = ((string)(dataRow["NOME"] == DBNull.Value ? null : dataRow["NOME"])).ToUpper();
            //        cli.Endereço = (string)(dataRow["END_LOGRAD"] == DBNull.Value ? null : dataRow["END_LOGRAD"])
            //                       + ", " +
            //                       (string)(dataRow["END_NUMERO"] == DBNull.Value ? null : dataRow["END_NUMERO"]);
            //        cli.Cidade = (string)(dataRow["CIDADE"] == DBNull.Value ? null : dataRow["CIDADE"]);
            //        cli.Estado = (string)(dataRow["UF"] == DBNull.Value ? null : dataRow["UF"]);
            //        cli.Bairro = (string)(dataRow["END_BAIRRO"] == DBNull.Value ? null : dataRow["END_BAIRRO"]);
            //        cli.CEP = (string)(dataRow["END_CEP"] == DBNull.Value ? null : dataRow["END_CEP"]);
            //        cli.Representante = (string)(dataRow["SOC_GERENTE"] == DBNull.Value ? null : dataRow["SOC_GERENTE"]);
            //        cli.CPFDoRepresentante = (string)(dataRow["INSC_MUNIC"] == DBNull.Value ? null : dataRow["INSC_MUNIC"]);
            //        cli.DataMelhorVencimento = (dataRow["DT_MELHOR_VENCTO"] == DBNull.Value ? 0 : (short)dataRow["DT_MELHOR_VENCTO"]);
            //        cli.IDFirebird = (int)(dataRow["ID_CLIENTE"]);
            //        //cli.Contratos = await _contratoDataService.GetAllAsNoTrackingByCliente(cli);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //        throw;
            //    }

            //    ClientesListFull.Add(cli);
            //}


        }
    }

    public class ExibeDiálogoGeraMensalidade : ICommand
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly IDialogsStore _dialogStore;
        private readonly ClienteStore _clienteStore;

        public ExibeDiálogoGeraMensalidade(HomeViewModel homeViewModel, IServiceProvider serviceProvider)
        {
            _homeViewModel = homeViewModel;
            _dialogStore = serviceProvider.GetRequiredService<IDialogsStore>();
            _clienteStore = serviceProvider.GetRequiredService<ClienteStore>();
            homeViewModel.PropertyChanged += HomeViewModel_PropertyChanged;
        }

        private void HomeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public bool CanExecute(object? parameter)
        {
            return _homeViewModel.ClienteSelecionado is not null;
        }

        public void Execute(object? parameter)
        {
            //_dialogGenerator.ViewModelExibido =
            //    _dialogVMFactory.CreateDialogContentViewModel(TipoDialog.GeraContasView);
            _clienteStore.Cliente = _homeViewModel.ClienteSelecionado;
            _dialogStore.RegisterDialog(TipoDialog.GeraContasView);
        }

        public event EventHandler? CanExecuteChanged;
    }

    public class SincronizarComBlingCommand : AsyncCommandBase
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly Progress<ProgressReport> _progressIndicator;
        private readonly Sincronizador sync;


        public SincronizarComBlingCommand(HomeViewModel homeViewModel, IServiceProvider serviceProvider, Progress<ProgressReport> progressIndicator, Action<Exception> onException) : base(onException)
        {
            _homeViewModel = homeViewModel;
            _progressIndicator = progressIndicator;
            _homeViewModel.PropertyChanged += _homeViewModel_PropertyChanged;
            sync = new(_homeViewModel, serviceProvider);
        }

        private void _homeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public override bool CanExecute(object parameter)
        {
            return _homeViewModel.HitTestVisible;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            _homeViewModel.HitTestVisible = false;
            _homeViewModel.CanvasVisibility = Visibility.Visible;
            await Task.Run(() => sync.RunSync(_progressIndicator));
        }


        public override event EventHandler CanExecuteChanged;
    }

    public class VisualizaPDVCommand : AsyncCommandBase
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly ContratoDataService _contratosDataService;


        public VisualizaPDVCommand(HomeViewModel homeViewModel, IServiceProvider serviceProvider, Action<Exception> onException) : base(onException)
        {
            _homeViewModel = homeViewModel;
            //_contratosDataService = serviceProvider.GetRequiredService<ContratoDataService>();
        }



        protected override async Task ExecuteAsync(object parameter)
        {
            var lista = await _contratosDataService.GetAllAsNoTrackingByCliente(_homeViewModel.ClienteSelecionado);
            var contrato = lista.Find(x => x.ContratoPDF is not null);
            if (contrato is not null)
                //Process.Start(contrato.ContratoPDF);
                return;
            else
                MessageBox.Show("O usuário não tem contrato assinado no sistema.");

        }

        public override event EventHandler? CanExecuteChanged;
    }
}
