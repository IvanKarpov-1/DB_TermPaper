using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Address;
using UI.Commands.Base;
using UI.Commands.Customer;
using UI.Commands.Order;
using UI.Commands.RepairRequest;
using UI.Services;
using UI.Stores;
using UI.ViewModels.Address;
using UI.ViewModels.Base;
using UI.ViewModels.Order;
using UI.ViewModels.RepairRequest;

namespace UI.ViewModels.Customer;

public class CustomerDetailsViewModel : ViewModelBase, ILoadingViewModel, INeedCustomerAddressesViewModel, IEditableViewModel
{
	private readonly int _customerId;

	public CustomerDetailsViewModel(
		int customerId,
		NavigationStore navigationStore,
		CustomerStore customerStore,
		AddressStore addressStore,
		OrderStore orderStore,
		RepairRequestStore repairRequestStore,
		NavigationService<CustomerListViewModel> customerListViewNavigationService,
		NavigationService<AddressDetailsViewModel> addressDetailsViewNavigationService,
		NavigationService<AddAddressViewModel> addAddressViewNavigationService,
		NavigationService<OrderDetailsViewModel> orderDetailsViewNavigationService,
		NavigationService<AddOrderViewModel> addOrderViewNavigationService,
		NavigationService<RepairRequestDetailsViewModel> repairRequestDetailsViewNavigationService,
		NavigationService<AddRepairRequestViewModel> addRepairRequestViewNavigationService,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_customerId = customerId;
		_addresses = new ObservableCollection<AddressListItemViewModel>();
		_orders = new ObservableCollection<OrderListItemViewModel>();
		_repairRequests = new ObservableCollection<RepairRequestListItemViewModel>();

		LoadCustomerByIdCommand = new LoadCustomerByIdCommand(this, customerStore);
		NavigateBackCommand = new NavigateCommand<CustomerListViewModel>(customerListViewNavigationService);
		EditCommand = new RelayCommand(OnEditExecuted, _ => true);
		SaveCommand = new UpdateCustomerCommand(this, customerStore, snackbarMessageQueue);
		CancelCommand = new RelayCommand(OnCancelExecuted, _ => true);
		DeleteCommand = new DeleteCustomerCommand(this, customerStore, navigationStore, snackbarMessageQueue);

		LoadCustomerAddressesCommand = new LoadCustomerAddressesCommand(this, addressStore, customerId);
		NavigateToAddressDetailsCommand = new NavigateCommand<AddressDetailsViewModel>(addressDetailsViewNavigationService);
		NavigateToAddAddressCommand = new NavigateCommand<AddAddressViewModel>(addAddressViewNavigationService);

		LoadOrdersCommand = new LoadOrdersCommand(this, orderStore, customerId);
		NavigateToOrderDetailsCommand = new NavigateCommand<OrderDetailsViewModel>(orderDetailsViewNavigationService);
		NavigateToAddOrderCommand = new NavigateCommand<AddOrderViewModel>(addOrderViewNavigationService);

		LoadRepairRequestsCommand = new LoadRepairRequestsCommand(this, repairRequestStore, customerId);
		NavigateToRepairRequestDetailsCommand = new NavigateCommand<RepairRequestDetailsViewModel>(repairRequestDetailsViewNavigationService);
		NavigateToAddRepairRequestCommand = new NavigateCommand<AddRepairRequestViewModel>(addRepairRequestViewNavigationService);
	}

	public static CustomerDetailsViewModel LoadViewModel(
		int customerId,
		NavigationStore navigationStore,
		CustomerStore customerStore,
		AddressStore addressStore,
		OrderStore orderStore,
		RepairRequestStore repairRequestStore,
		NavigationService<CustomerListViewModel> customerListViewNavigationService,
		NavigationService<AddressDetailsViewModel> addressDetailsViewNavigationService,
		NavigationService<AddAddressViewModel> addAddressViewNavigationService,
		NavigationService<OrderDetailsViewModel> orderDetailsViewNavigationService,
		NavigationService<AddOrderViewModel> addOrderViewNavigationService,
		NavigationService<RepairRequestDetailsViewModel> repairRequestDetailsViewNavigationService,
		NavigationService<AddRepairRequestViewModel> addRepairRequestViewNavigationService,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		var viewModel = new CustomerDetailsViewModel(
			customerId,
			navigationStore,
			customerStore,
			addressStore,
			orderStore,
			repairRequestStore,
			customerListViewNavigationService,
			addressDetailsViewNavigationService,
			addAddressViewNavigationService,
			orderDetailsViewNavigationService,
			addOrderViewNavigationService,
			repairRequestDetailsViewNavigationService,
			addRepairRequestViewNavigationService,
			snackbarMessageQueue);
		viewModel.Load();
		return viewModel;
	}

	public async void Load()
	{
		await LoadCustomerByIdCommand.ExecuteAsync(_customerId);
		await LoadCustomerAddressesCommand.ExecuteAsync(null);
		await LoadOrdersCommand.ExecuteAsync(null);
		await LoadRepairRequestsCommand.ExecuteAsync(null);
	}

	private CustomerListItemViewModel _customer = null!;
	private CustomerListItemViewModel _tempCustomer = null!;
	public CustomerListItemViewModel Customer
	{
		get => _customer;
		set => SetField(ref _customer, value);
	}
	
	public void UpdateCustomer(Domain.Models.Customer customer)
	{
		Customer = new CustomerListItemViewModel(customer);
		_isCustomerLoading = false;
		OnPropertyChanged(nameof(IsLoading));
	}

	private bool _isCustomerLoading = true;
	private bool _isAddressLoading = true;
	private bool _isOrderLoading = true;
	private bool _isRepairLoading = true;

	public bool IsLoading => _isCustomerLoading || _isAddressLoading || _isOrderLoading || _isRepairLoading;

	private bool _isEditing;
	public bool IsEditing
	{
		get => _isEditing;
		set => SetField(ref _isEditing, value);
	}

	public AsyncCommandBase LoadCustomerByIdCommand { get; }
	public ICommand NavigateBackCommand { get; }
	public ICommand EditCommand { get; }
	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
	public ICommand DeleteCommand { get; }

	private void OnEditExecuted(object? p)
	{
		IsEditing = true;
		_tempCustomer = new CustomerListItemViewModel(Customer.Customer);
	}

	private void OnCancelExecuted(object? p)
	{
		IsEditing = false;
		Customer = _tempCustomer;
	}

	#region Addresses

	private readonly ObservableCollection<AddressListItemViewModel> _addresses;
	public IEnumerable<AddressListItemViewModel> Addresses => _addresses;

	public AsyncCommandBase LoadCustomerAddressesCommand { get; }
	public ICommand NavigateToAddressDetailsCommand { get; }
	public ICommand NavigateToAddAddressCommand { get; }

	public void UpdateAddresses(IEnumerable<Domain.Models.Address> addresses)
	{
		addresses = addresses.OrderBy(x => x.Id);

		_addresses.Clear();

		foreach (var address in addresses)
		{
			var addressListItemViewModel = new AddressListItemViewModel(address);
			_addresses.Add(addressListItemViewModel);
			addressListItemViewModel.PropertyChanged += OnIsAddressSelectedPropertyChanged;
		}

		_isAddressLoading = false;
		OnPropertyChanged(nameof(IsLoading));
	}

	public bool? IsAllAddressesSelected
	{
		get
		{
			if (_addresses.Count == 0) return false;
			var selected = _addresses.Select(x => x.IsSelected).Distinct().ToList();
			return selected.Count == 1 ? selected.Single() : null;
		}
		set
		{
			if (!value.HasValue) return;
			SelectAll(value.Value, _addresses);
			OnPropertyChanged();
		}
	}

	private void OnIsAddressSelectedPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(AddressListItemViewModel.IsSelected))
			OnPropertyChanged(nameof(IsAllAddressesSelected));
	}

	#endregion

	#region Orders

	private readonly ObservableCollection<OrderListItemViewModel> _orders;
	public IEnumerable<OrderListItemViewModel> Orders => _orders;

	public AsyncCommandBase LoadOrdersCommand { get; }
	public ICommand NavigateToOrderDetailsCommand { get; }
	public ICommand NavigateToAddOrderCommand { get; }

	public void UpdateOrders(IEnumerable<Domain.Models.Order?> orders)
	{
		orders = orders.OrderBy(x => x.Id);

		_orders.Clear();

		foreach (var order in orders)
		{
			if (order == null) continue;

			var address = Addresses.FirstOrDefault(x => x.Id == order.AddressId)?.AddressString;

			var orderListItemViewModel = new OrderListItemViewModel(order, address);
			_orders.Add(orderListItemViewModel);
			orderListItemViewModel.PropertyChanged += OnIsOrderSelectedPropertyChanged;
		}

		_isOrderLoading = false;
		OnPropertyChanged(nameof(IsLoading));
	}

	public bool? IsAllOrdersSelected
	{
		get
		{
			if (_orders.Count == 0) return false;
			var selected = _orders.Select(x => x.IsSelected).Distinct().ToList();
			return selected.Count == 1 ? selected.Single() : null;
		}
		set
		{
			if (!value.HasValue) return;
			SelectAll(value.Value, _orders);
			OnPropertyChanged();
		}
	}

	private void OnIsOrderSelectedPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(OrderListItemViewModel.IsSelected))
			OnPropertyChanged(nameof(IsAllOrdersSelected));
	}

	#endregion

	#region RepairRequests

	private readonly ObservableCollection<RepairRequestListItemViewModel> _repairRequests;
	public IEnumerable<RepairRequestListItemViewModel> RepairRequests => _repairRequests;

	public AsyncCommandBase LoadRepairRequestsCommand { get; }
	public ICommand NavigateToRepairRequestDetailsCommand { get; }
	public ICommand NavigateToAddRepairRequestCommand { get; }

	public void UpdateRepairRequests(IEnumerable<Domain.Models.RepairRequest?> repairRequests)
	{
		repairRequests = repairRequests.OrderBy(x => x.Id);

		_repairRequests.Clear();

		foreach (var repairRequest in repairRequests)
		{
			if (repairRequest == null) continue;
			
			var repairRequestListItem = new RepairRequestListItemViewModel(repairRequest);
			_repairRequests.Add(repairRequestListItem);
			repairRequestListItem.PropertyChanged += OnIsRepairRequestSelectedPropertyChanged;
		}

		_isRepairLoading = false;
		OnPropertyChanged(nameof(IsLoading));
	}

	public bool? IsAllRepairRequestsSelected
	{
		get
		{
			if (_repairRequests.Count == 0) return false;
			var selected = _repairRequests.Select(x => x.IsSelected).Distinct().ToList();
			return selected.Count == 1 ? selected.Single() : null;
		}
		set
		{
			if (!value.HasValue) return;
			SelectAll(value.Value, _repairRequests);
			OnPropertyChanged();
		}
	}

	private void OnIsRepairRequestSelectedPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(RepairRequestListItemViewModel.IsSelected))
			OnPropertyChanged(nameof(IsAllRepairRequestsSelected));
	}

	#endregion

	private static void SelectAll<TListItemViewModel>(bool select, IEnumerable<TListItemViewModel> items) where TListItemViewModel : class, ISelectableListItem
	{
		foreach (var item in items)
		{
			item.IsSelected = select;
		}
	}
}