using ContratosMVVM.Commands;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ContratosMVVM.ViewModels.DialogViewModels
{
    public class EditaContratosBaseViewModel : DialogContentViewModelBase
    {
        private readonly ContratoBaseDataService _contratoBaseDataService;
        private readonly SetorDataService _setorDataService;
        private CONTRATO_BASE _contratoBaseSelecionado = new ();
        private ObservableCollection<CONTRATO_BASE> _contratosBaseList = new();
        private ObservableCollection<SETOR> _setores = new();
        private ICommand _fecharJanela;
        private ICommand _adicionaContratoNovo;
        private ICommand _editaSelecionado;

        public ICommand SalvarContratoBase { get; set; }

        public ObservableCollection<CONTRATO_BASE> ContratosBaseList
        {
            get => _contratosBaseList;
            set => _contratosBaseList = value;
        }

        public ObservableCollection<SETOR> Setores
        {
            get => _setores;
            set => _setores = value;
        }

        public CONTRATO_BASE ContratoBaseSelecionado
        {
            get => _contratoBaseSelecionado;
            set
            {
                if (value is null) return;
                _contratoBaseSelecionado = value;
                OnPropertyChanged(nameof(ContratoBaseSelecionado));
            }
        }

        public ICommand FecharJanela
        {
            get => _fecharJanela;
            set => _fecharJanela = value;
        }

        public ICommand AdicionaContratoNovo
        {
            get => _adicionaContratoNovo;
            set => _adicionaContratoNovo = value;
        }

        public ICommand EditaSelecionado
        {
            get => _editaSelecionado;
            set => _editaSelecionado = value;
        }

        public EditaContratosBaseViewModel(IServiceProvider serviceProvider)
        {
            _contratoBaseDataService = serviceProvider.GetRequiredService<ContratoBaseDataService>();
            _setorDataService = serviceProvider.GetRequiredService<SetorDataService>();
            EditaSelecionado = new EditaContratobaseSelecionado(this, serviceProvider, (x) => MessageBox.Show(x.Message));
            SalvarContratoBase = new SalvarContratoBaseCommand(this, serviceProvider, (x) => MessageBox.Show(x.Message));
            AdicionaContratoNovo = new AdicionaNovoContratoCommand(this);
            FecharJanela = new FecharJanelaAbertaCommand(serviceProvider);
            CarregaInformações();
        }

        public void AtualizaListagem()
        {
            CarregaInformações();
        }
        private async Task CarregaInformações()
        {
            Setores.Clear();
            ContratosBaseList.Clear();
            foreach (var contratoBase in await _contratoBaseDataService.GetAllAsNoTracking())
            {
                ContratosBaseList.Add(contratoBase);
            }

            foreach (var setor in await _setorDataService.GetAllAsNoTracking())
            {
                Setores.Add(setor);
            }

        }
    }
}
