using System.Windows.Input;
using UI.Commands;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.ViewModels;

public class HomePageViewModel : ViewModelBase
{
	private readonly AnalyticsStore _analyticsStore;

	public HomePageViewModel(AnalyticsStore analyticsStore)
	{
		_analyticsStore = analyticsStore;
		LoadAnalyticsCommand = new LoadAnalyticsCommand(this, _analyticsStore);
	}

	private decimal _totalEstimatedOutput;
	private int _numberOfProductsPerYear;
	private int _numberOfOrders;

	public decimal TotalEstimatedOutput
	{
		get => _totalEstimatedOutput;
		set => SetField(ref _totalEstimatedOutput, value);
	}

	public int NumberOfProductsPerYear
	{
		get => _numberOfProductsPerYear;
		set => SetField(ref _numberOfProductsPerYear, value);
	}

	public int NumberOfOrders
	{
		get => _numberOfOrders; 
		set => SetField(ref _numberOfOrders, value);
	}

	public ICommand LoadAnalyticsCommand { get; }

	public static HomePageViewModel LoadViewModel(AnalyticsStore analyticsStore)
	{
		var viewModel = new HomePageViewModel(analyticsStore);
		viewModel.LoadAnalyticsCommand.Execute(null);
		return viewModel;
	}
}