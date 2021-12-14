using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using ContratosMVVM.Auxiliares;
using ContratosMVVM.Context;
using ContratosMVVM.Domain;
using ContratosMVVM.Services;
using ContratosMVVM.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContratosMVVM.Methods
{
    public class Sincronizador
    {
        private readonly IServiceProvider serviços;
        private readonly HomeViewModel _parentViewModel;


        public Sincronizador(HomeViewModel parentViewModel, IServiceProvider provedor)
        {
            serviços = provedor;
            _parentViewModel = parentViewModel;
        }

        public async Task RunSync(IProgress<ProgressReport> progress)
        {
            BlingAPIService api = serviços.GetRequiredService<BlingAPIService>();
            ContatoDataService contatoDataService = serviços.GetRequiredService<ContatoDataService>();
            INIFileService iniFile = new();


            OperationResponse response;

            //List<CLIENTE> contatosServidor = null;
            //OperationResponse contatosResponse;
            //progress.Report(new ProgressReport("Bling", "Contatos", "Carregando dados a sincronizar"));
            //(contatosResponse, contatosServidor) = await BaixaClientesDoBling.Execute(api, progress,
            //    DateTime.Parse(iniFile.Get(INIConfig.UltimaSync)));
            //if (!contatosResponse.IsSuccessful)
            //{
            //    progress.Report(new("Bling", "Contatos do Bling", $"{contatosResponse.ErrorMessage}"));
            //    //log.Error("Contatos do Bling", contatosResponse.Exception);
            //    return;
            //}


            //for (int i = 0; i < contatosServidor.Count; i++)
            //{
            //    progress.Report(new ProgressReport(null,
            //        $"Contato {i + 1} de {contatosServidor.Count + 1}",
            //        $"{contatosServidor[i].RazãoSocial}"));
            //    response = await PUTContatoNoLocal.Execute(contatosServidor[i],
            //        contatoDataService);
            //    if (!response.IsSuccessful)
            //    {
            //        progress.Report(new(null, "Contatos", $"{response.ErrorMessage}"));
            //        //log.Warn("Contato para a base local falhou", response.Exception);
            //        continue;
            //    }
            //}

            try
            {
                await contatoDataService.SaveStage();
            }
            catch (Exception e)
            {
                //log.Error("SaveStage Contatos final falhou", e);
            }


            //iniFile.Set(INIConfig.UltimaSync, $"{DateTime.Now:dd/MM/yyyy HH:mm}");

        }

        //public void LeArquivoINI()
        //{
        //    IP_LIST.Clear();
        //    string line;
        //    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        //        "Trilha Informatica", $"Synch"));
        //    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        //        "Trilha Informatica", $"Synch\\terminais.ini"))) File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Trilha Informatica", $"Synch\\terminais.ini"));
        //    StreamReader sr = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Trilha Informatica", $"Synch\\terminais.ini"));
        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        IP_LIST.Add(line);
        //    }
        //}
    }

}
