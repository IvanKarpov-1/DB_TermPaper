using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using UI.Commands;
using UI.Services;
using UI.Stores;
using UI.ViewModels.Base;
using UI.ViewModels.Customer;
using UI.ViewModels.Factory;
using UI.ViewModels.Product;

namespace UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
	private readonly NavigationStore _navigationStore;

	public MainWindowViewModel(NavigationStore navigationStore,
		NavigationService<HomePageViewModel> homePageViewNavigationService,
		NavigationService<CustomerListViewModel> customerListViewNavigationService,
		NavigationService<FactoryListViewModel> factoryListViewNavigationService,
		NavigationService<ProductListViewModel> productListViewNavigationService, 
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_navigationStore = navigationStore;
		_snackbarMessageQueue = snackbarMessageQueue;
		
		_navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

		NavigateBackCommand = new NavigateBackCommand(navigationStore);
		NavigateForwardCommand = new NavigateForwardCommand(navigationStore);
		NavigateToHomePageCommand = new NavigateCommand<HomePageViewModel>(homePageViewNavigationService);
		NavigateToCustomersPageCommand = new NavigateCommand<CustomerListViewModel>(customerListViewNavigationService);
		NavigateToFactoriesPageCommand = new NavigateCommand<FactoryListViewModel>(factoryListViewNavigationService);
		NavigateToProductsPageCommand = new NavigateCommand<ProductListViewModel>(productListViewNavigationService);
	}

	private void OnCurrentViewModelChanged()
	{
		OnPropertyChanged(nameof(CurrentViewModel));
	}

	public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel!;

	private readonly ISnackbarMessageQueue _snackbarMessageQueue;
	public ISnackbarMessageQueue SnackbarMessageQueue => _snackbarMessageQueue;

	public ICommand NavigateBackCommand { get; }
	public ICommand NavigateForwardCommand { get; }
	public ICommand NavigateToHomePageCommand { get;}
	public ICommand NavigateToCustomersPageCommand { get;}
	public ICommand NavigateToFactoriesPageCommand { get;}
	public ICommand NavigateToProductsPageCommand { get; }
}