using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels;

namespace UI.Commands;

public class LoadAnalyticsCommand : AsyncCommandBase
{
	private readonly HomePageViewModel _homePageViewModel;
	private readonly AnalyticsStore _analyticsStore;

	public LoadAnalyticsCommand(HomePageViewModel homePageViewModel, AnalyticsStore analyticsStore)
	{
		_homePageViewModel = homePageViewModel;
		_analyticsStore = analyticsStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		await _analyticsStore.Load();
		_homePageViewModel.TotalEstimatedOutput = _analyticsStore.TotalEstimatedOutput;
		_homePageViewModel.NumberOfProductsPerYear = _analyticsStore.NumberOfProductsPerYear;
		_homePageViewModel.NumberOfOrders = _analyticsStore.NumberOfOrders;
	}
}