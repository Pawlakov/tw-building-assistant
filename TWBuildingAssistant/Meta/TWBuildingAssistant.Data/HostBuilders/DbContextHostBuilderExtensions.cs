namespace TWBuildingAssistant.Data.HostBuilders;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.Data.Sqlite;

public static class DbContextHostBuilderExtensions
{
    public static IHostBuilder AddDbContext(this IHostBuilder host)
    {
        host.ConfigureServices((context, services) =>
        {
            string connectionString = context.Configuration.GetConnectionString("sqlserver");
            Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlServer(connectionString);

            services.AddDbContext<DatabaseContext>(configureDbContext);
            services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));
        });

        return host;
    }
}