using Domain.Models;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Address;
using UI.Commands.Base;
using UI.Commands.Order;
using UI.Services;
using UI.Stores;
using UI.ViewModels.Address;
using UI.ViewModels.Base;
using UI.ViewModels.Product;

namespace UI.ViewModels.Order;

public class OrderDetailsViewModel : ViewModelBase, ILoadingViewModel, IEditableViewModel, INeedCustomerAddressesViewModel
{
	private readonly int _orderId;

	public OrderDetailsViewModel(
		int orderId,
		int customerId,
		NavigationStore navigationStore,
		OrderStore orderStore,
		AddressStore addressStore,
		ProductStore productStore,
		NavigationService<ProductDetailsViewModel> productDetailsViewNavigationService,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_orderId = orderId;
		_orderDetails = new ObservableCollection<OrderDetailListItemViewModel>();
		_addresses = new ObservableCollection<AddressListItemViewModel>();
		_selectedAddress = new AddressListItemViewModel(new Domain.Models.Address());
		CustomerId = customerId;

		LoadOrderByIdCommand = new LoadOrderByIdCommand(this, orderStore);
		NavigateBackCommand = new NavigateBackCommand(navigationStore);
		EditCommand = new RelayCommand(OnEditExecuted, _ => true);
		SaveCommand = new UpdateOrderCommand(this, orderStore, snackbarMessageQueue);
		CancelCommand = new RelayCommand(OnCancelExecuted, _ => true);
		DeleteCommand = new DeleteOrderCommand(this, orderStore, navigationStore, snackbarMessageQueue);

		LoadProductsCommand = new LoadProductsCommand(this, productStore);
		NavigateToProductDetailsCommand = new NavigateCommand<ProductDetailsViewModel>(productDetailsViewNavigationService);

		LoadCustomerAddressesCommand = new LoadCustomerAddressesCommand(this, addressStore, customerId);
	}

	public static OrderDetailsViewModel LoadViewModel(
		int orderId,
		int customerId,
		NavigationStore navigationStore,
		OrderStore orderStore,
		AddressStore addressStore,
		ProductStore productStore,
		NavigationService<ProductDetailsViewModel> productDetailsViewNavigationService,
		ISnackbarMessageQueue snackbarMessageQueue
	)
	{
		var viewModel = new OrderDetailsViewModel(
			orderId,
			customerId,
			navigationStore,
			orderStore,
			addressStore,
			productStore,
			productDetailsViewNavigationService,
			snackbarMessageQueue);
		viewModel.Load();
		return viewModel;
	}

	public async void Load()
	{
		await LoadOrderByIdCommand.ExecuteAsync(_orderId);
		await LoadProductsCommand.ExecuteAsync(null);
		await LoadCustomerAddressesCommand.ExecuteAsync(null);
	}

	public int CustomerId { get; }

	private OrderListItemViewModel _order = null!;
	private OrderListItemViewModel _tempOrder = null!;
	public OrderListItemViewModel Order
	{
		get => _order;
		set => SetField(ref _order, value);
	}

	private bool _isOrderLoading = true;
	private bool _isAddressesLoading = true;
	private bool _isOrderDetailsLoading = true;
	public bool IsLoading => _isOrderLoading || _isAddressesLoading || _isOrderDetailsLoading;

	private bool _isEditing;
	public bool IsEditing
	{
		get => _isEditing;
		set => SetField(ref _isEditing, value);
	}

	public AsyncCommandBase LoadOrderByIdCommand { get; }
	public ICommand NavigateBackCommand { get; }
	public ICommand EditCommand { get; }
	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
	public ICommand DeleteCommand { get; }

	private void OnEditExecuted(object? p)
	{
		IsEditing = true;
		_tempOrder = new OrderListItemViewModel(Order.GetOrder(), "");
	}

	private void OnCancelExecuted(object? p)
	{
		IsEditing = false;
		Order = _tempOrder;
	}

	public void UpdateOrder(Domain.Models.Order order)
	{
		Order = new OrderListItemViewModel(order, "");
		_isOrderLoading = false;
		OnPropertyChanged(nameof(IsLoading));
	}

	private readonly ObservableCollection<OrderDetailListItemViewModel> _orderDetails;
	public IEnumerable<OrderDetailListItemViewModel> OrderDetails => _orderDetails;

	public AsyncCommandBase LoadProductsCommand { get; }
	public ICommand NavigateToProductDetailsCommand { get; }

	public void UpdateOrderDetails(List<(Domain.Models.Product?, int)> orderDetails)
	{
		orderDetails = orderDetails.OrderBy(x => x.Item1?.Id).ToList();

		_orderDetails.Clear();

		foreach (var orderDetailListItemViewModel in orderDetails.Select(orderDetail => 
			         new OrderDetailListItemViewModel(orderDetail.Item1 ?? new Domain.Models.Product(),
						new OrderDetail { Quantity = orderDetail.Item2 })))
		{
			_orderDetails.Add(orderDetailListItemViewModel);
			orderDetailListItemViewModel.PropertyChanged += OnIsSelectedPropertyChanged;
		}

		_isOrderDetailsLoading = false;
		OnPropertyChanged(nameof(IsLoading));
	}

	public bool? IsAllItemsSelected
	{
		get
		{
			if (_orderDetails.Count == 0) return false;
			var selected = _orderDetails.Select(x => x.IsSelected).Distinct().ToList();
			return selected.Count == 1 ? selected.Single() : null;
		}
		set
		{
			if (!value.HasValue) return;
			SelectAll(value.Value, _orderDetails);
			OnPropertyChanged();
		}
	}

	private static void SelectAll(bool select, IEnumerable<OrderDetailListItemViewModel> products)
	{
		foreach (var product in products)
		{
			product.IsSelected = select;
		}
	}

	private void OnIsSelectedPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(OrderDetailListItemViewModel.IsSelected))
			OnPropertyChanged(nameof(IsAllItemsSelected));
	}

	public AsyncCommandBase LoadCustomerAddressesCommand { get; }

	private readonly ObservableCollection<AddressListItemViewModel> _addresses;
	public IEnumerable<AddressListItemViewModel> Addresses => _addresses;

	private AddressListItemViewModel _selectedAddress;
	public AddressListItemViewModel SelectedAddress
	{
		get => _selectedAddress;
		set => SetField(ref _selectedAddress, value);
	}

	public void UpdateAddresses(IEnumerable<Domain.Models.Address> addresses)
	{
		addresses = addresses.OrderBy(x => x.Id);

		_addresses.Clear();

		foreach (var address in addresses)
		{
			var addressListItemViewModel = new AddressListItemViewModel(address);
			_addresses.Add(addressListItemViewModel);
		}

		var selectedAddress = Addresses.FirstOrDefault(x => x.Id == Order.Order.AddressId)?.Address;
		SelectedAddress = new AddressListItemViewModel(selectedAddress ?? new Domain.Models.Address());

		_isAddressesLoading = false;
		OnPropertyChanged(nameof(IsLoading));
	}
}