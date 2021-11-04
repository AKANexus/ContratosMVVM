
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ContratosMVVM.Context;
using ContratosMVVM.Services;

namespace ContratosMVVM.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            INIFileService iNiFileService = new INIFileService();
            host.ConfigureServices((context, serviços) =>
            {
                //string connString = $"server={iNiFileService.Get(INIConfig.MySQLIP)};userid=AmbiStore;password=masterkey;database=Cobranca;ConvertZeroDateTime=True";
                //ServerVersion version = new MySqlServerVersion(new Version(8, 0, 23));
                //Action<DbContextOptionsBuilder> configureDbContext = c =>
                //{
                //    c.UseMySql(connString, version, x =>
                //    {
                //        x.CommandTimeout(600);
                //        x.EnableRetryOnFailure(3);
                //    });
                //    c.EnableSensitiveDataLogging();
                //    //c.UseLazyLoadingProxies();
                //};


                string connString = "Data Source=Servidor.db";
                Action<DbContextOptionsBuilder> configureDbContext = c =>
                {
                    c.UseSqlite(connString, x =>
                    {
                        x.CommandTimeout(600);
                    });
                    c.EnableSensitiveDataLogging();
                };

                serviços.AddSingleton(new CobrancaDbContextFactory(configureDbContext));
                serviços.AddDbContext<CobrancaDbContext>(configureDbContext);
            });

            return host;
        }
    }
}
