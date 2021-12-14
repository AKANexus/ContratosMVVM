using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Auxiliares;
using ContratosMVVM.Domain;

namespace ContratosMVVM.Services
{
    public class BaixaClientesDoBling
    {
        public static async Task<(OperationResponse, List<CLIENTE>)> Execute(BlingAPIService api, IProgress<ProgressReport> progress, DateTime últimaSync)
        {
            List<CLIENTE> contatosServidor = new();

            List<Contato2> allContatos;
            try
            {
                allContatos = await api.GETAllContatos(progress, últimaSync, DateTime.Now);
                var x = allContatos.Where(x => x.Fantasia == "Batman");
            }
            catch (Exception e)
            {
                return (new(false, $"Falha ao executar GETAllContatos: {e.Message}", e), null);
            }
            try
            {
                for (int i = 0; i < allContatos.Count; i++)
                {
                    progress.Report(new ProgressReport("Bling", "Itens", $"Lendo contato {allContatos[i].Nome} - {i + 1} de {allContatos.Count}"));
                    if (string.IsNullOrWhiteSpace(allContatos[i].Cnpj) || allContatos[i].Cnpj == "Não identificado") continue;
                    CLIENTE novoContato = new()
                    {
                        RazãoSocial = allContatos[i].Nome,
                        CNPJCPF = allContatos[i].Cnpj
                    };
                    
                    contatosServidor.Add(novoContato);
                }
            }
            catch (Exception e)
            {
                return (new OperationResponse(false, $"Falha ao gerar Contatos a partir dos do Bling", e), null);
            }
            return (new(true, ""), contatosServidor);
        }
    }
}
