using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ContratosMVVM.ViewModels.DialogViewModels;

namespace ContratosMVVM.Commands
{
    public class GerarContasAReceberCommand : AsyncCommandBase
    {
        private readonly GeraContasViewModel _geraContasViewModel;
        private readonly CLIENTE _cliente;
        private HomeViewModel _homeViewModel;
        private List<CLIENTE> _clientesList = new();
        private readonly ContratoDataService _contratoDataService;
        private static readonly FbConnection FbConnection = new(@"initial catalog=D:\PROGRAMAS\CompuFour\Clipp_ASSISTENCIA\Base\CLIPP.FDB;data source=192.168.10.250;user id=SYSDBA;password=masterke;encoding=WIN1252;charset=utf8");
        private readonly FbCommand _fbCommand = new() { Connection = FbConnection, CommandType = CommandType.Text };


        public GerarContasAReceberCommand(GeraContasViewModel geraContasViewModel, IServiceProvider serviceProvider, CLIENTE cliente, Action<Exception> onException) : base(onException)
        {
            _geraContasViewModel = geraContasViewModel;
            _cliente = cliente;
            _contratoDataService = serviceProvider.GetRequiredService<ContratoDataService>();
            _homeViewModel = serviceProvider.GetRequiredService<HomeViewModel>();

        }

        protected override async Task ExecuteAsync(object parameter)
        {

            if (_cliente is null)
            {
                _clientesList = _homeViewModel.ClientesListFull;
            }
            else
            {
                _clientesList.Add(_cliente);
            }

            try
            {
                await FbConnection.OpenAsync();
                foreach (var cliente in _clientesList)
                {
                    var contratos = await _contratoDataService.GetAllAsNoTrackingByCliente(cliente);
                    var valorConta = contratos.Sum(x => x.ValorTotalDoContrato);

                    await GeraContaAReceber(valorConta, cliente);


                }

                MessageBox.Show("Done!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                await FbConnection.CloseAsync();
            }
        }

        private async Task<bool> GeraContaAReceber(decimal valor, CLIENTE cliente, bool avisaRepetido = true)
        {
            var vencimentoManual = !string.IsNullOrWhiteSpace(_geraContasViewModel.DiaVencimento);
            DateTime vencimento;
            switch (vencimentoManual)
            {
                case true:
                    if (!DateTime.TryParse(_geraContasViewModel.DiaVencimento + "-" + _geraContasViewModel.Mes + "-" + _geraContasViewModel.Ano, out vencimento))
                    {
                        MessageBox.Show("Valor incorreto de data informado");
                        return false;
                    }
                    else
                    {
                        if (vencimento.CompareTo(DateTime.Today) <= 0)
                        {
                            MessageBox.Show("A o vencimento é para hoje ou uma data anterior.");
                            return false;
                        }
                    }
                    break;
                default:

                    if (cliente.DataMelhorVencimento is 0)
                    {
                        MessageBox.Show("Verifique se o valor de melhor vencimento está preenchido no Clipp.");
                        //tentativas += 1;
                        return false;
                    }
                    if (!DateTime.TryParse(cliente.DataMelhorVencimento + "-" + _geraContasViewModel.Mes + "-" + _geraContasViewModel.Ano, out vencimento))
                    {
                        MessageBox.Show("Valor incorreto de data informado.");
                        return false;
                    }
                    else
                    {
                        if (vencimento.CompareTo(DateTime.Today) <= 0)
                        {
                            MessageBox.Show("A o vencimento é para hoje ou uma data anterior.");
                            return false;
                        }
                    }

                    break;

            }

            var idClienteString = cliente.IDFirebird.ToString();
            _fbCommand.CommandText = "SELECT NEXT VALUE FOR GEN_TB_CTAREC_ID FROM RDB$DATABASE";
            var idContareceber = Convert.ToInt32(await _fbCommand.ExecuteScalarAsync());
            _fbCommand.CommandText = "SELECT NEXT VALUE FOR GEN_TB_MOVDIARIO_ID FROM RDB$DATABASE";
            int idMovdiario = Convert.ToInt32(await _fbCommand.ExecuteScalarAsync());

            int idMovdiarioSeg = Convert.ToInt32(await _fbCommand.ExecuteScalarAsync());

            if (idClienteString.StartsWith("9")) idClienteString = $"X{idClienteString[1..]}";
            string DOCUMENTO = idClienteString + _geraContasViewModel.Ano + _geraContasViewModel.Mes.PadLeft(2, '0');
            string INVREFERENCIA = $"D{idContareceber.ToString().PadLeft(4, '0')[(idClienteString.PadLeft(5, '0').Length - 4)..]}{DOCUMENTO}";
            decimal valor_dec = Convert.ToDecimal(valor);

            _fbCommand.CommandText = @$"SELECT COUNT(*) FROM TB_CONTA_RECEBER WHERE DOCUMENTO = '{DOCUMENTO}' AND TIP_CTAREC = 'D'";
            if ((int)(await _fbCommand.ExecuteScalarAsync() ?? 0) > 0)
            {
                //if (avisaRepetido)
                //{
                //    MessageBox.Show("Essa conta já foi gerada para esse cliente");
                //}
                return false;
            }
            if (valor_dec <= 0)
            {
                //MessageBox.Show("O valor do contrato não está definido na base de dados.");
                return false;
            }
            try
            {
                DateTime referencia = vencimento.AddMonths(-1);
                _fbCommand.CommandText = $@"INSERT INTO ""TB_CONTA_RECEBER"" (""ID_CTAREC"", ""DOCUMENTO"", ""HISTORICO"", ""DT_EMISSAO"", ""DT_VENCTO"", ""VLR_CTAREC"", ""TIP_CTAREC"", ""ID_PORTADOR"", ""ID_CLIENTE"", ""INV_REFERENCIA"", ""DT_VENCTO_ORIG"", ""NSU_CARTAO"") VALUES ({idContareceber}, {DOCUMENTO}, '{$"MENSALID. AUTOM. REF. {referencia.Month.ToString()}-{referencia.Year.ToString().Substring(2)}"}', '{DateTime.Today:yyyy-MM-dd}', '{vencimento:yyyy-MM-dd}', {valor_dec.ToString("F2", CultureInfo.InvariantCulture)}, '{"D"}', {"4"}, {idClienteString}, '{INVREFERENCIA}', '{vencimento:yyyy-MM-dd}', NULL)";
                await _fbCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("AAA ao gravar conta a receber. \n\nSe o problema persistir, por favor entre em contato com a equipe de suporte.");
                MessageBox.Show(ex.Message);
                if (!String.IsNullOrWhiteSpace(ex.InnerException.Message)) MessageBox.Show(ex.InnerException.Message);
                return false;
            }

            /*if (result_contarec != 1)
            {
                MessageBox.Show("AAA ao gravar conta a receber (FinalizaNoSATLocal - AAA ao gerar ContaRec). \n\nPor favor entre em contato com a equipe de suporte.");
                return;
            }*/

            try
            {


                _fbCommand.CommandText =
                    $"INSERT INTO \"TB_MOVDIARIO\" (\"ID_MOVTO\", \"DT_MOVTO\", \"HR_MOVTO\", \"HISTORICO\", \"TIP_MOVTO\", \"VLR_MOVTO\", \"ID_CTAPLA\", \"SYNCED\", \"ID_MOVTO_VINC\") " +
                    $"VALUES ({idMovdiario}, '{DateTime.Today:yyyy-MM-dd}', '{DateTime.Now:hh:mm:ss}', 'MENSALIDADE DOC {DOCUMENTO}','C', {valor_dec.ToString("F2", CultureInfo.InvariantCulture)}, 148, NULL, NULL)";
                await _fbCommand.ExecuteNonQueryAsync();

                _fbCommand.CommandText =
                    $"INSERT INTO \"TB_MOVDIARIO\" (\"ID_MOVTO\", \"DT_MOVTO\", \"HR_MOVTO\", \"HISTORICO\", \"TIP_MOVTO\", \"VLR_MOVTO\", \"ID_CTAPLA\", \"SYNCED\", \"ID_MOVTO_VINC\") " +
                    $"VALUES ({idMovdiarioSeg}, '{DateTime.Today:yyyy-MM-dd}', '{DateTime.Now:hh:mm:ss}', 'DOC {DOCUMENTO} {cliente.RazãoSocial}', 'D', {valor_dec.ToString("F2", CultureInfo.InvariantCulture)}, 30, NULL, NULL)";
                await _fbCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("AAA ao gravar movimentação referente à conta a receber. \n\nSe o problema persistir, por favor entre em contato com a equipe de suporte.");
                MessageBox.Show(ex.Message);
                if (!String.IsNullOrWhiteSpace(ex.InnerException.Message)) MessageBox.Show(ex.InnerException.Message);

                return false;
            }
            /*
            if (result_movdiario != 1)
            {
                MessageBox.Show("AAA ao gravar movimentação financeira (FinalizaNoSATLocal - AAA ao gerar MovDiario). \n\nPor favor entre em contato com a equipe de suporte.");
                return;
            }
            */
            try
            {
                _fbCommand.CommandText =
                    $"INSERT INTO \"TB_CTAREC_MOVTO\" (\"ID_MOVTO\", \"ID_CTAREC\") VALUES ({idMovdiario}, {idContareceber})";
                await _fbCommand.ExecuteNonQueryAsync();
                _fbCommand.CommandText =
                    $"INSERT INTO \"TB_CTAREC_MOVTO\" (\"ID_MOVTO\", \"ID_CTAREC\") VALUES ({idMovdiarioSeg}, {idContareceber})";
                await _fbCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("AAA ao gravar movimentação/conta a receber. \n\nSe o problema persistir, por favor entre em contato com a equipe de suporte.");
                MessageBox.Show(ex.Message);
                if (!String.IsNullOrWhiteSpace(ex.InnerException.Message)) MessageBox.Show(ex.InnerException.Message);
                return false;
            }

            return true;
        }

    }
}
