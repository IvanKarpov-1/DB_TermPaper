using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Base;
using UI.Commands.Product;
using UI.Services;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.ViewModels.Product;

public class ProductListViewModel : ViewModelBase, ILoadingViewModel
{
	public ProductListViewModel(
		ProductStore productStore,
		NavigationService<ProductDetailsViewModel> productDetailsViewNavigationService,
		NavigationService<AddProductViewModel> addProductViewNavigationService)
	{
		_products = new ObservableCollection<ProductListItemViewModel>();
		
		LoadProductsCommand = new LoadProductsCommand(this, productStore);
		NavigateToProductDetailsCommand = new NavigateCommand<ProductDetailsViewModel>(productDetailsViewNavigationService);
		NavigateToAddProductCommand = new NavigateCommand<AddProductViewModel>(addProductViewNavigationService);
	}

	public static ProductListViewModel LoadViewModel(
		ProductStore productStore,
		NavigationService<ProductDetailsViewModel> productDetailsViewNavigationService,
		NavigationService<AddProductViewModel> addProductViewNavigationService)
	{
		var viewModel = new ProductListViewModel(
			productStore,
			productDetailsViewNavigationService,
			addProductViewNavigationService);
		viewModel.Load();
		return viewModel;
	}

	public async void Load()
	{
		//await Task.Delay(5000);
		await LoadProductsCommand.ExecuteAsync(null);
	}

	private bool _isLoading = true;
	public bool IsLoading
	{
		get => _isLoading;
		set => SetField(ref _isLoading, value);
	}

	private readonly ObservableCollection<ProductListItemViewModel> _products;
	public IEnumerable<ProductListItemViewModel> Products => _products;

	public AsyncCommandBase LoadProductsCommand { get; }
	public ICommand NavigateToProductDetailsCommand { get; }
	public ICommand NavigateToAddProductCommand { get; }

	public bool? IsAllItemsSelected
	{
		get
		{
			if (_products.Count == 0) return false;
			var selected = _products.Select(x => x.IsSelected).Distinct().ToList();
			return selected.Count == 1 ? selected.Single() : null;
		}
		set
		{
			if (!value.HasValue) return;
			SelectAll(value.Value, _products);
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

	private void OnIsSelectedPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(ProductListItemViewModel.IsSelected))
			OnPropertyChanged(nameof(IsAllItemsSelected));
	}

	public void UpdateProducts(IEnumerable<Domain.Models.Product> products)
	{
		products = products.OrderBy(x => x.Id);

		_products.Clear();

		foreach (var product in products)
		{
			var productListItemViewModel = new ProductListItemViewModel(product);
			_products.Add(productListItemViewModel);
			productListItemViewModel.PropertyChanged += OnIsSelectedPropertyChanged;
		}

		IsLoading = false;
	}
}