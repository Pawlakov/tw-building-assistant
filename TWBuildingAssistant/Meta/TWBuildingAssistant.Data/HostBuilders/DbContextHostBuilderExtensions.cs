namespace TWBuildingAssistant.Data.HostBuilders;

using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.Data.Sqlite;

public static class DbContextHostBuilderExtensions
{
    public static IHostBuilder AddDbContextLocal(this IHostBuilder host)
    {
        host.ConfigureServices((context, services) =>
        {
            var connectionString = context.Configuration.GetConnectionString("sqlserver");
            Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlServer(connectionString);

            services.AddDbContext<DatabaseContext>(configureDbContext);
            services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));
        });

        return host;
    }

    public static IHostBuilder AddDbContextAzure(this IHostBuilder host)
    {
        host.ConfigureServices((context, services) =>
        {
            var client = new SecretClient(new Uri("https://twa-keys.vault.azure.net/"), new DefaultAzureCredential());
            var secret = client.GetSecret("twa-connstring");
            var connectionString = secret.Value.Value;
            Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlServer(connectionString);

            services.AddDbContext<DatabaseContext>(configureDbContext);
            services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));
        });

        return host;
    }
}