using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContratosMVVM.Context;
using ContratosMVVM.Generics;
using ContratosMVVM.HostBuilders;
using ContratosMVVM.Services;
using ContratosMVVM.States;
using ContratosMVVM.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ContratosMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        private readonly Logger log = new("Startup");

        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        private static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddConfiguration()
                .AddDbContext()
                .AddServices()
                .AddStores()
                .AddViewModels()
                .AddViews()
                ;

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var mutexStore = _host.Services.GetRequiredService<MutexStore>();
            string mutexName = "Contratos_" + System.Security.Principal.WindowsIdentity.GetCurrent().User.AccountDomainSid;


            if (Mutex.TryOpenExisting(mutexName, out Mutex result))
            {
                MessageBox.Show("O programa já está aberto. Procure na barra de tarefas. \n Ɛ>");
                PostMessage(
                    (IntPtr)HWND_BROADCAST,
                    WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
                Shutdown();
                return;
            }

            mutexStore.AppMutex = new Mutex(true, mutexName);


            Logger.Start(new FileInfo($"./Logs/LOG-{DateTime.Today:dd-MM-yy}.log"));

            var contextFactory = _host.Services.GetRequiredService<CobrancaDbContextFactory>();
            var context = contextFactory.CreateDbContext();
            context.Database.EnsureCreated();


            ActualStartup();
        }


        private async Task ActualStartup()
        {
            /*
            log.Info("Preparando serviços");
            IDialogGenerator dialogGenerator = _host.Services.GetRequiredService<IDialogGenerator>();
            IDialogsStore dialogStore = _host.Services.GetRequiredService<IDialogsStore>();
            IDialogViewModelFactory dialogVMFactory = _host.Services.GetRequiredService<IDialogViewModelFactory>();
            IMessaging<(string, string)> messaging = _host.Services.GetRequiredService<IMessaging<(string, string)>>();
            
            messaging.Mensagem = ("Aguarde...", "Atualizando tabelas...");

            dialogGenerator.ViewModelExibido = dialogVMFactory.CreateDialogContentViewModel(TipoDialogue.WIP);
            dialogStore.RegisterDialog(dialogGenerator, "DISGRAÇA", false);
            CobrancaDbContext context;
            try
            {
                CobrancaDbContextFactory contextFactory = _host.Services.GetRequiredService<CobrancaDbContextFactory>();
                context = contextFactory.CreateDbContext();
            }
            catch (Exception ex)
            {
                log.Error("Falha ao registrar Contexts");
                log.Error($"{ex.Message}");
                dialogStore.CloseDialog(DialogResult.OK);
                MessageBox.Show($"Falha ao registrar Contexts: \n {ex.Message}");
                Environment.Exit(0);
                log.Info("Fechando sistema. Tchau.");
                throw;
            }
            log.Info("Contextos instanciados...");
            Task task =
            Task.Run(() =>
            {
                log.Info("Executando MIGRATE");

                context.Database.Migrate();

                log.Info("Migrate executado");
                context.Dispose();
            }
                );
            try
            {
                log.Info("Iniciando MIGRATE");
                await Task.Run(() => task);
                log.Info("MIGRATE executado com sucesso!");
            }
            catch (Exception ex)
            {
                log.Error("Falha ao executar MIGRATE");
                log.Error($"{ex.Message}");
                dialogStore.CloseDialog(DialogResult.OK);
                MessageBox.Show($"Falha ao atualizar as tabelas: \n {ex.Message}");
                Environment.Exit(0);
                log.Info("Fechando sistema. Tchau.");
                throw;
            }
            */
            try
            {
                log.Info("Abrindo <MainView>");
                Window janela = _host.Services.GetRequiredService<MainView>();
                janela.Show();
            }
            catch (Exception ex)
            {
                log.Error("Falha ao registrar <MainView>");
                log.Error($"{ex.Message}");
                //dialogStore.CloseDialog(DialogResult.OK);
                MessageBox.Show($"Falha ao registrar <MainView>: \n {ex.Message}");
                Environment.Exit(0);
                //log.Info("Fechando sistema. Tchau.");
                throw;
            }
            finally
            {
                log.Info("Fechando tela de load");
                //dialogStore.CloseDialog(DialogResult.OK);
            }
        }


        protected override async void OnExit(ExitEventArgs e)
        {
            var mutexStore = _host.Services.GetRequiredService<MutexStore>();
            if (mutexStore.AppMutex is not null && mutexStore.AppMutex.WaitOne(5000, false))
                mutexStore.AppMutex.ReleaseMutex();
            await _host.StopAsync();
            _host.Dispose();
            Logger.ShutDown();
            base.OnExit(e);
        }
    }
}
