using Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using System.Windows;
using UI.Services;
using UI.ViewModels;

namespace UI;

public partial class App : System.Windows.Application
{
	private readonly IHost _host;

	public App()
	{
		_host = Host.CreateDefaultBuilder()
			.ConfigureServices((context, services) =>
			{
				services.AddApplicationServices();
				services.AddPersistenceServices(context.Configuration);
				services.AddUiServices();
			})
			.Build();
	}

	protected override async void OnStartup(StartupEventArgs e)
	{
		await _host.StartAsync();

		var navigationService = _host.Services.GetRequiredService<NavigationService<HomePageViewModel>>();
		navigationService.Navigate(null);

		MainWindow = _host.Services.GetRequiredService<MainWindow>();
		MainWindow.Show();

		base.OnStartup(e);
	}

	protected override async void OnExit(ExitEventArgs e)
	{
		await _host.StopAsync();
		_host.Dispose();

		base.OnExit(e);
	}
}