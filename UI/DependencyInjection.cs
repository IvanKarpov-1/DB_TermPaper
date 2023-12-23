using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using UI.Services;
using UI.Stores;
using UI.ViewModels;
using UI.ViewModels.Address;
using UI.ViewModels.Customer;
using UI.ViewModels.Factory;
using UI.ViewModels.Order;
using UI.ViewModels.Product;
using UI.ViewModels.RepairRequest;

namespace UI;

public static class DependencyInjection
{
	public static IServiceCollection AddUiServices(this IServiceCollection services)
	{
		services.AddTransient(CreateHomePageViewModel);
		services.AddSingleton<Func<object?, HomePageViewModel>>(s =>
		{
			return _ => s.GetRequiredService<HomePageViewModel>();
		});
		services.AddSingleton<NavigationService<HomePageViewModel>>();

		#region ProductViewModels

		services.AddTransient(CreateProductListViewModel);
		services.AddSingleton<Func<object?, ProductListViewModel>>(s => {
			return _ => s.GetRequiredService<ProductListViewModel>();
		});
		services.AddSingleton<NavigationService<ProductListViewModel>>();
		
		services.AddTransient<Func<object?, ProductDetailsViewModel>>(s => {
			return productId => CreateProductDetailsViewModel(s, productId);
		});
		services.AddSingleton<NavigationService<ProductDetailsViewModel>>();
		
		services.AddTransient<Func<object?, AddProductViewModel>>(s => {
			return factoryId => CreateAddProductViewModel(s, factoryId);
		});
		services.AddSingleton<NavigationService<AddProductViewModel>>();

		#endregion

		#region FactoryViewModels

		services.AddTransient(CreateFactoryListViewModel);
		services.AddSingleton<Func<object?, FactoryListViewModel>>(s => {
			return _ => s.GetRequiredService<FactoryListViewModel>();
		});
		services.AddSingleton<NavigationService<FactoryListViewModel>>();

		services.AddTransient<Func<object?, FactoryDetailsViewModel>>(s => {
			return factoryId => CreateFactoryDetailsViewModel(s, factoryId!);
		});
		services.AddSingleton<NavigationService<FactoryDetailsViewModel>>();

		services.AddTransient<AddFactoryViewModel>();
		services.AddSingleton<Func<object?, AddFactoryViewModel>>(s => {
			return _ => s.GetRequiredService<AddFactoryViewModel>();
		});
		services.AddSingleton<NavigationService<AddFactoryViewModel>>();

		#endregion

		#region CustomerViewModels

		services.AddTransient(CreateCustomerListViewModel);
		services.AddSingleton<Func<object?, CustomerListViewModel>>(s => {
			return _ => s.GetRequiredService<CustomerListViewModel>();
		});
		services.AddSingleton<NavigationService<CustomerListViewModel>>();

		services.AddTransient<Func<object?, CustomerDetailsViewModel>>(s => {
			return parameter => CreateCustomerDetailsViewModel(s, parameter!);
		});
		services.AddSingleton<NavigationService<CustomerDetailsViewModel>>();

		services.AddTransient<AddCustomerViewModel>();
		services.AddSingleton<Func<object?, AddCustomerViewModel>>(s => {
			return _ => s.GetRequiredService<AddCustomerViewModel>();
		});
		services.AddSingleton<NavigationService<AddCustomerViewModel>>();

		#endregion

		#region AddressViewModels

		services.AddTransient<Func<object?, AddressDetailsViewModel>>(s => {
			return addressId => CreateAddressDetailsViewModel(s, addressId!);
		});
		services.AddSingleton<NavigationService<AddressDetailsViewModel>>();
		
		services.AddTransient<Func<object?, AddAddressViewModel>>(s => {
			return customerId => CreateAddAddressViewModel(s, customerId!);
		});
		services.AddSingleton<NavigationService<AddAddressViewModel>>();

		#endregion

		#region OrderViewModels

		services.AddTransient<Func<object?, OrderDetailsViewModel>>(s => {
			return customerId => CreateOrderDetailsViewModel(s, customerId!);
		});
		services.AddSingleton<NavigationService<OrderDetailsViewModel>>();

		services.AddTransient<Func<object?, AddOrderViewModel>>(s => {
			return customerId => CreateAddOrderViewModel(s, customerId!);
		});
		services.AddSingleton<NavigationService<AddOrderViewModel>>();

		#endregion

		#region RepaireRequestViewModels

		services.AddTransient<Func<object?, RepairRequestDetailsViewModel>>(s => {
			return customerId => CreateRepairRequestDetailsViewModel(s, customerId!);
		});
		services.AddSingleton<NavigationService<RepairRequestDetailsViewModel>>();

		services.AddTransient<Func<object?, AddRepairRequestViewModel>>(s => {
			return customerId => CreateAddRepairRequestViewModel(s, customerId!);
		});
		services.AddSingleton<NavigationService<AddRepairRequestViewModel>>();

		#endregion

		services.AddSingleton<NavigationStore>();
		services.AddSingleton<ProductStore>();
		services.AddSingleton<AnalyticsStore>();
		services.AddSingleton<FactoryStore>();
		services.AddSingleton<CustomerStore>();
		services.AddSingleton<AddressStore>();
		services.AddSingleton<OrderStore>();
		services.AddSingleton<RepairRequestStore>();

		services.AddSingleton<MainWindowViewModel>();

		services.AddSingleton(s => new MainWindow
		{
			DataContext = s.GetRequiredService<MainWindowViewModel>()
		});

		services.AddSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>();

		return services;
	}

	private static HomePageViewModel CreateHomePageViewModel(IServiceProvider serviceProvider)
	{
		return HomePageViewModel.LoadViewModel(
			serviceProvider.GetRequiredService<AnalyticsStore>());
	}

	private static ProductListViewModel CreateProductListViewModel(IServiceProvider serviceProvider)
	{
		return ProductListViewModel.LoadViewModel(
			serviceProvider.GetRequiredService<ProductStore>(),
			serviceProvider.GetRequiredService<NavigationService<ProductDetailsViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<AddProductViewModel>>());
	}

	private static ProductDetailsViewModel CreateProductDetailsViewModel(IServiceProvider serviceProvider,
		object? productId)
	{
		return ProductDetailsViewModel.LoadViewModel(
			(int)productId!,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<ProductStore>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}

	private static AddProductViewModel CreateAddProductViewModel(IServiceProvider serviceProvider, object? factoryId)
	{
		return AddProductViewModel.LoadViewModel(
			factoryId,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<ProductStore>(),
			serviceProvider.GetRequiredService<FactoryStore>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}

	private static FactoryListViewModel CreateFactoryListViewModel(IServiceProvider serviceProvider)
	{
		return FactoryListViewModel.LoadViewModel(
			serviceProvider.GetRequiredService<FactoryStore>(),
			serviceProvider.GetRequiredService<NavigationService<FactoryDetailsViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<AddFactoryViewModel>>());
	}

	private static FactoryDetailsViewModel CreateFactoryDetailsViewModel(IServiceProvider serviceProvider,
		object factoryId)
	{
		return FactoryDetailsViewModel.LoadViewModel(
			(int)factoryId,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<FactoryStore>(),
			serviceProvider.GetRequiredService<ProductStore>(),
			serviceProvider.GetRequiredService<NavigationService<ProductDetailsViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<AddProductViewModel>>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}

	private static CustomerListViewModel CreateCustomerListViewModel(IServiceProvider serviceProvider)
	{
		return CustomerListViewModel.LoadViewModel(
			serviceProvider.GetRequiredService<CustomerStore>(),
			serviceProvider.GetRequiredService<NavigationService<CustomerDetailsViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<AddCustomerViewModel>>());
	}

	private static CustomerDetailsViewModel CreateCustomerDetailsViewModel(IServiceProvider serviceProvider,
		object customerId)
	{
		return CustomerDetailsViewModel.LoadViewModel(
			(int)customerId,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<CustomerStore>(),
			serviceProvider.GetRequiredService<AddressStore>(),
			serviceProvider.GetRequiredService<OrderStore>(),
			serviceProvider.GetRequiredService<RepairRequestStore>(),
			serviceProvider.GetRequiredService<NavigationService<CustomerListViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<AddressDetailsViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<AddAddressViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<OrderDetailsViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<AddOrderViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<RepairRequestDetailsViewModel>>(),
			serviceProvider.GetRequiredService<NavigationService<AddRepairRequestViewModel>>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}

	private static AddressDetailsViewModel CreateAddressDetailsViewModel(IServiceProvider serviceProvider,
		object addressId)
	{
		return AddressDetailsViewModel.LoadViewModel(
			(int)addressId,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<AddressStore>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}

	private static AddAddressViewModel CreateAddAddressViewModel(IServiceProvider serviceProvider,
		object customerId)
	{
		return AddAddressViewModel.LoadViewModel(
			(int)customerId,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<AddressStore>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}

	private static AddOrderViewModel CreateAddOrderViewModel(IServiceProvider serviceProvider, object customerId)
	{
		return AddOrderViewModel.LoadViewModel(
			(int)customerId,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<OrderStore>(),
			serviceProvider.GetRequiredService<AddressStore>(),
			serviceProvider.GetRequiredService<ProductStore>(),
			serviceProvider.GetRequiredService<NavigationService<ProductDetailsViewModel>>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}

	private static OrderDetailsViewModel CreateOrderDetailsViewModel(IServiceProvider serviceProvider,
		object parameters)
	{
		var parametersDictionary = (Dictionary<string, object>)parameters;

		var orderId = (int)parametersDictionary["p1"];
		var customerId = (int)parametersDictionary["p2"];

		return OrderDetailsViewModel.LoadViewModel(
			orderId,
			customerId,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<OrderStore>(),
			serviceProvider.GetRequiredService<AddressStore>(),
			serviceProvider.GetRequiredService<ProductStore>(),
			serviceProvider.GetRequiredService<NavigationService<ProductDetailsViewModel>>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}

	private static AddRepairRequestViewModel CreateAddRepairRequestViewModel(IServiceProvider serviceProvider, object customerId)
	{
		return AddRepairRequestViewModel.LoadViewModel(
			(int)customerId,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<RepairRequestStore>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}

	private static RepairRequestDetailsViewModel CreateRepairRequestDetailsViewModel(IServiceProvider serviceProvider,
		object customerId)
	{
		return RepairRequestDetailsViewModel.LoadViewModel(
			(int)customerId,
			serviceProvider.GetRequiredService<NavigationStore>(),
			serviceProvider.GetRequiredService<RepairRequestStore>(),
			serviceProvider.GetRequiredService<ISnackbarMessageQueue>());
	}
}