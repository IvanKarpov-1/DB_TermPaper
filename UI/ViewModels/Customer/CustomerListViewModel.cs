using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Base;
using UI.Commands.Customer;
using UI.Services;
using UI.Stores;
using UI.ViewModels.Base;
using UI.ViewModels.Product;

namespace UI.ViewModels.Customer;

public class CustomerListViewModel : ViewModelBase, ILoadingViewModel
{
	public CustomerListViewModel(
		CustomerStore customerStore,
		NavigationService<CustomerDetailsViewModel> customerDetailsViewNavigateService,
		NavigationService<AddCustomerViewModel> addCustomerViewNavigationService)
	{
		_customers = new ObservableCollection<CustomerListItemViewModel>();

		LoadCustomersCommand = new LoadCustomersCommand(this, customerStore);
		NavigateToCustomerDetailsCommand = new NavigateCommand<CustomerDetailsViewModel>(customerDetailsViewNavigateService);
		NavigateToAddCustomerCommand = new NavigateCommand<AddCustomerViewModel>(addCustomerViewNavigationService);
	}

	public static CustomerListViewModel LoadViewModel(
		CustomerStore customerStore,
		NavigationService<CustomerDetailsViewModel> customerDetailsViewNavigateService,
		NavigationService<AddCustomerViewModel> addCustomerViewNavigationService)
	{
		var viewModel = new CustomerListViewModel(
			customerStore,
			customerDetailsViewNavigateService,
			addCustomerViewNavigationService);
		viewModel.Load();
		return viewModel;
	}

	public async void Load()
	{
		await LoadCustomersCommand.ExecuteAsync(null);
	}

	private bool _isLoading = true;
	public bool IsLoading
	{
		get => _isLoading;
		set => SetField(ref _isLoading, value);
	}

	private readonly ObservableCollection<CustomerListItemViewModel> _customers;
	public IEnumerable<CustomerListItemViewModel> Customers => _customers;

	public AsyncCommandBase LoadCustomersCommand { get; }
	public ICommand NavigateToCustomerDetailsCommand { get; }
	public ICommand NavigateToAddCustomerCommand { get; }

	public bool? IsAllItemsSelected
	{
		get
		{
			if (_customers.Count == 0) return false;
			var selected = _customers.Select(x => x.IsSelected).Distinct().ToList();
			return selected.Count == 1 ? selected.Single() : null;
		}
		set
		{
			if (!value.HasValue) return;
			SelectAll(value.Value, _customers);
			OnPropertyChanged();
		}
	}

	private static void SelectAll(bool select, IEnumerable<CustomerListItemViewModel> customers)
	{
		foreach (var product in customers)
		{
			product.IsSelected = select;
		}
	}

	private void OnCustomerAdded(Domain.Models.Customer customer)
	{
		var productListItemViewModel = new CustomerListItemViewModel(customer);
		_customers.Add(productListItemViewModel);
		productListItemViewModel.PropertyChanged += OnIsSelectedPropertyChanged;
	}

	private void OnCustomerDeleted(int customerId)
	{
		_customers.RemoveAt(_customers.IndexOf(_customers.FirstOrDefault(x => x.Id == customerId)!));
	}

	private void OnIsSelectedPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(ProductListItemViewModel.IsSelected))
			OnPropertyChanged(nameof(IsAllItemsSelected));
	}

	public void UpdateCustomers(IEnumerable<Domain.Models.Customer> customers)
	{
		customers = customers.OrderBy(x => x.Id);

		_customers.Clear();

		foreach (var customer in customers)
		{
			var productListItemViewModel = new CustomerListItemViewModel(customer);
			_customers.Add(productListItemViewModel);
			productListItemViewModel.PropertyChanged += OnIsSelectedPropertyChanged;
		}

		IsLoading = false;
	}
}