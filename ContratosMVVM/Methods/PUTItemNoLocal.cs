using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Auxiliares;
using ContratosMVVM.Context;
using ContratosMVVM.Domain;
using ContratosMVVM.Services;

namespace ContratosMVVM.Methods
{
    public static class PUTContatoNoLocal
    {
        public static async Task<OperationResponse> Execute(CLIENTE contato,
            ContatoDataService contatoLocalDataService)
        {
            try
            {
                CLIENTE tentativo = await contatoLocalDataService.GetAsNoTracking(contato.CNPJCPF);
                if (tentativo is null)
                {
                    tentativo = new()
                    {
                        
                    };
                    
                    if (await contatoLocalDataService.AddToCreateStage(tentativo) > 100)
                    {
                        await contatoLocalDataService.SaveStage();
                    }
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                return new(false, $"Falha ao sincronizar o contato: {contato.RazãoSocial}", e);
            }
            return new(true, "", null);
        }
    }

}
