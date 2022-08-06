﻿namespace TWBuildingAssistant.WorldManager;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using TWBuildingAssistant.Data.HostBuilders;
using TWBuildingAssistant.WorldManager.HostBuilders;
using TWBuildingAssistant.WorldManager.ViewModels;
using TWBuildingAssistant.WorldManager.Views;

public partial class App 
    : Application
{
    private readonly IHost host;

    public App()
    {
        this.host = CreateHostBuilder().Build();
    }

    public static IHostBuilder CreateHostBuilder(string[] args = null)
    {
        return Host.CreateDefaultBuilder(args)
            .AddConfiguration()
            .AddDbContext()
            /*.AddServices()
            .AddStores()*/
            .AddViewModels()
            .AddViews();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        this.host.Start();

        var window = this.host.Services.GetRequiredService<MainWindow>();
        window.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await this.host.StopAsync();
        this.host.Dispose();

        base.OnExit(e);
    }
}
