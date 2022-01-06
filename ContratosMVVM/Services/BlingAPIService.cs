using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Auxiliares;
using ContratosMVVM.Domain;
using ContratosMVVM.Generics;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serialization.Json;

namespace ContratosMVVM.Services
{
    public class BlingAPIService
    {
        private readonly string _apikey;// = "a1189ea2a9e0bb4c31ecf9568b3ee95a5813ab16193c19e12a3624da2a2df4b542a9ed0f";
        private readonly string _numLoja;
        private Logger log = new("API");
        public BlingAPIService(IServiceProvider serviceProvider)
        {
            var iniFile = serviceProvider.GetRequiredService<INIFileService>();
            _apikey = iniFile.Get(INIConfig.APIKey);
            _numLoja = iniFile.Get(INIConfig.NumLoja);
            log.Debug($"ApiKey = {_apikey}");
            log.Debug($"NumLoja = {_numLoja}");
        }

        public async Task<List<Contato2>> GETAllContatos(IProgress<ProgressReport> progresso, DateTime startDate = default, DateTime endDate = default)
        {
            List<Contato2> AllContatos = new();
            DateTime today = DateTime.Today;
            var client = new RestClient();
            client.Timeout = 15000;
            var request = new RestRequest(Method.GET);
            int currentAttempt = 1;

            int page = 1;
            bool hasData = true;
            JsonDeserializer deserial = new();

            while (hasData)
            {
                progresso.Report(new ProgressReport("Bling", "Contatos", $"Baixando página {page} de contatos."));

                IRestResponse response;
                try
                {
                    if (startDate != default && endDate != default)
                    {
                        client.BaseUrl =
                            new Uri(
                                @$"https://bling.com.br/Api/v2/contatos/page={page}/json?apikey={_apikey}&filters=dataInclusao[{startDate:dd/MM/yyyy} TO {endDate:dd/MM/yyyy}]");
                    }
                    else
                    {
                        client.BaseUrl =
                            new Uri(
                                @$"https://bling.com.br/Api/v2/contatos/page={page}/json?apikey={_apikey}");
                    }

                    response = await client.ExecuteAsync(request);

                }
                catch (Exception e)
                {
                    log.Error($"GETAllContatos lançou uma exceção em \"client.Execute\":");
                    log.Error(e.Message);
                    break;
                }

                RootContato reply;

                while (!response.IsSuccessful && currentAttempt < 11)
                {
                    await Task.Delay(1000);
                    response = await client.ExecuteAsync(request);
                    progresso.Report(new ProgressReport("Bling", "Contatos", $"Pág {page}, tentativa {currentAttempt}"));
                    currentAttempt++;
                }

                if (response.IsSuccessful)
                {
                    reply = deserial.Deserialize<RootContato>(response);
                    currentAttempt = 1;
                }
                else
                {
                    log.Error($"GETAllContatos lançou uma exceção em \"deserial.Deserialize\":");
                    log.Error("response was not sucessful");
                    log.Error($"{response.ErrorMessage}");
                    break;
                }

                if (reply.Retorno.Erros is not null)
                {
                    hasData = false;
                }
                else
                {
                    foreach (var contatoNode in reply.Retorno.Contatos)
                    {
                        AllContatos.Add(contatoNode.Contato);
                    }
                    page++;
                }
            }
            return AllContatos;
        }

        public async Task<bool> POSTContaReceber(IProgress<ProgressReport> progresso, CLIENTE cliente, DateTime vencimento)
        {
            var client = new RestClient();
            client.Timeout = 15000;
            var request = new RestRequest(Method.POST);
            int currentAttempt = 1;
            JsonDeserializer deserial = new();

            progresso.Report(new ProgressReport("Bling", "ContaReceber", $"Gravando conta a receber com vencimento em {vencimento}."));
            var xmlContaRec = $"<contareceber><valor>{cliente.Contratos.Sum(x=>x.ValorTotalDoContrato)}</valor><vencimentoOriginal>{vencimento:dd/MM/yyyy}</vencimentoOriginal><historico>Ref. à mensalidade de prestação de serviços</historico><ocorrencia><ocorrenciaTipo>U</ocorrenciaTipo></ocorrencia><cliente><nome>{cliente.RazãoSocial}</nome><cpf_cnpj>{cliente.CNPJCPF}</cpf_cnpj></cliente></contareceber>";

            IRestResponse response;
            try
            {

                client.BaseUrl =
                    new Uri(
                        @$"https://bling.com.br/b/Api/v2/contareceber/json/?apikey={_apikey}&xml={xmlContaRec}");


                response = await client.ExecuteAsync(request);

            }
            catch (Exception e)
            {
                log.Error($"POSTContaReceber lançou uma exceção em \"client.Execute\":");
                log.Error(e.Message);
                return false;
            }

            RootContato reply = new();

            while (!response.IsSuccessful && currentAttempt < 11)
            {
                await Task.Delay(1000);
                response = await client.ExecuteAsync(request);
                progresso.Report(new ProgressReport("Bling", "Contatos", $"Tentando novamente {currentAttempt}/10"));
                currentAttempt++;
            }

            if (response.IsSuccessful)
            {
                reply = deserial.Deserialize<RootContato>(response);
            }
            else
            {
                log.Error($"POSTContaReceber lançou uma exceção em \"client.Execute\":");
            }

            if (reply.Retorno.Erros is not null)
            {
                progresso.Report(new("Bling", "ContaReceber", reply.Retorno.Erros[0].Msg));
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
