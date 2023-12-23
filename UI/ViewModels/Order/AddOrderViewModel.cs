using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Domain.Models;
using MaterialDesignThemes.Wpf;
using UI.Commands;
using UI.Commands.Address;
using UI.Commands.Base;
using UI.Commands.Order;
using UI.Commands.Product;
using UI.Services;
using UI.Stores;
using UI.ViewModels.Address;
using UI.ViewModels.Base;
using UI.ViewModels.Product;

namespace UI.ViewModels.Order;

public class AddOrderViewModel : ViewModelBase, INeedCustomerAddressesViewModel, ILoadingViewModel
{
	private readonly int _customerId;

	public AddOrderViewModel(
		int customerId,
		NavigationStore navigationStore,
		OrderStore orderStore,
		AddressStore addressStore,
		ProductStore productStore,
		NavigationService<ProductDetailsViewModel> productDetailsViewNavigationService,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_customerId = customerId;
		_order = new OrderListItemViewModel(new Domain.Models.Order(), "");
		_orderDetails = new ObservableCollection<OrderDetailListItemViewModel>();
		_matchedProducts = new ObservableCollection<ProductListItemViewModel>();
		_addresses = new ObservableCollection<AddressListItemViewModel>();
		_selectedAddress = new AddressListItemViewModel(new Domain.Models.Address());

		CancelCommand = new NavigateBackCommand(navigationStore);
		SaveCommand = new AddOrderCommand(this, orderStore, snackbarMessageQueue);
		NavigateToProductDetailsCommand = new NavigateCommand<ProductDetailsViewModel>(productDetailsViewNavigationService);

		SearchProductCommand = new GetProductsByNameCommand(this, productStore);
		AddMatchedProductCommand = new RelayCommand(OnAddMatchedProductCommandExecute, _ => true);

		LoadCustomerAddressesCommand = new LoadCustomerAddressesCommand(this, addressStore, customerId);
	}

	public static AddOrderViewModel LoadViewModel(
		int customerId,
		NavigationStore navigationStore,
		OrderStore orderStore,
		AddressStore addressStore,
		ProductStore productStore,
		NavigationService<ProductDetailsViewModel> productDetailsViewNavigationService,
		ISnackbarMessageQueue snackbarMessageQueue
	)
	{
		var viewModel = new AddOrderViewModel(
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
		await LoadCustomerAddressesCommand.ExecuteAsync(null);
	}

	private OrderListItemViewModel _order;
	public OrderListItemViewModel Order
	{
		get => _order;
		set => SetField(ref _order, value);
	}

	private bool _isLoading = true;
	public bool IsLoading
	{
		get => _isLoading;
		set => SetField(ref _isLoading, value);
	}

	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
	public ICommand NavigateToProductDetailsCommand { get; }

	public Domain.Models.Order GetOrder()
	{
		var orderDetails = new List<OrderDetail>();

		foreach (var detailListItemViewModel in OrderDetails)
		{
			var product = detailListItemViewModel.GetProduct();
			orderDetails.Add(new OrderDetail
			{
				ProductId = product.Id,
				Quantity = detailListItemViewModel.Quantity,
				TotalPrice = detailListItemViewModel.Quantity * (decimal)product.SellingPrice!
			});
		}

		Order.Order.OrderDetails = orderDetails;
		Order.Order.CustomerId = _customerId;
		Order.Order.AddressId = SelectedAddress.Id;

		return Order.Order;
	}

	private readonly ObservableCollection<OrderDetailListItemViewModel> _orderDetails;
	public IEnumerable<OrderDetailListItemViewModel> OrderDetails => _orderDetails;

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

	private void OnIsSelectedPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(ProductListItemViewModel.IsSelected))
			OnPropertyChanged(nameof(IsAllItemsSelected));
	}

	public void UpdateProducts(IEnumerable<Domain.Models.Product> products)
	{
		products = products.OrderBy(x => x.Id);

		_orderDetails.Clear();

		foreach (var product in products)
		{
			var productListItemViewModel = new OrderDetailListItemViewModel(product, new OrderDetail());
			_orderDetails.Add(productListItemViewModel);
			productListItemViewModel.PropertyChanged += OnIsSelectedPropertyChanged;
		}
	}

	private readonly ObservableCollection<ProductListItemViewModel> _matchedProducts;
	public IEnumerable<ProductListItemViewModel> MatchedProducts => _matchedProducts;

	public ICommand SearchProductCommand { get; }
	public ICommand AddMatchedProductCommand { get; }

	public bool? IsMatchedProductsSelected
	{
		get
		{
			if (_matchedProducts.Count == 0) return false;
			var selected = _matchedProducts.Select(x => x.IsSelected).Distinct().ToList();
			return selected.Count == 1 ? selected.Single() : null;
		}
		set
		{
			if (!value.HasValue) return;
			SelectAll(value.Value, _matchedProducts);
			OnPropertyChanged();
		}
	}

	private static void SelectAll(bool select, IEnumerable<ProductListItemViewModel> products)
	{
		foreach (var product in products)
		{
			product.IsSelected = select;
		}
	}

	private void OnIsMatchedProductsSelectedPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(ProductListItemViewModel.IsSelected))
			OnPropertyChanged(nameof(IsMatchedProductsSelected));
	}

	public void UpdateMatchedProducts(IEnumerable<Domain.Models.Product> products)
	{
		products = products.OrderBy(x => x.Id);

		_matchedProducts.Clear();

		foreach (var product in products)
		{
			var productListItemViewModel = new ProductListItemViewModel(product);
			_matchedProducts.Add(productListItemViewModel);
			productListItemViewModel.PropertyChanged += OnIsMatchedProductsSelectedPropertyChanged;
		}
	}

	private string _productName = "";
	public string ProductName
	{
		get => _productName;
		set => SetField(ref _productName, value);
	}

	public void OnAddMatchedProductCommandExecute(object? p)
	{
		var tempMatchedProducts = _matchedProducts.ToList();

		foreach (var matchedProduct in tempMatchedProducts.Where(matchedProduct => matchedProduct.IsSelected))
		{
			_matchedProducts.Remove(matchedProduct);
			_orderDetails.Add(new OrderDetailListItemViewModel(matchedProduct.Product, new OrderDetail()));
		}
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
	}
}