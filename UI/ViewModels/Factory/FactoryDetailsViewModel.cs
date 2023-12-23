using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Base;
using UI.Commands.Factory;
using UI.Services;
using UI.Stores;
using UI.ViewModels.Base;
using UI.ViewModels.Product;

namespace UI.ViewModels.Factory;

public class FactoryDetailsViewModel : ViewModelBase, ILoadingViewModel, IEditableViewModel
{
	private readonly int _factoryId;

	public FactoryDetailsViewModel(
		int factoryId, 
		NavigationStore navigationStore,
		FactoryStore factoryStore,
		ProductStore productStore,
		NavigationService<ProductDetailsViewModel> productDetailsViewNavigationService,
		NavigationService<AddProductViewModel> addProductViewNavigationService,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_factoryId = factoryId;
		_products = new ObservableCollection<ProductListItemViewModel>();

		LoadFactoryByIdCommand = new LoadFactoryByIdCommand(this, factoryStore);
		NavigateBackCommand = new NavigateBackCommand(navigationStore);
		EditCommand = new RelayCommand(OnEditExecuted, _ => true);
		SaveCommand = new UpdateFactoryCommand(this, factoryStore, snackbarMessageQueue);
		CancelCommand = new RelayCommand(OnCancelExecuted, _ => true);
		DeleteCommand = new DeleteFactoryCommand(this, factoryStore, navigationStore, snackbarMessageQueue);

		LoadProductsCommand = new LoadProductsCommand(this, productStore, factoryId);
		NavigateToProductDetailsCommand = new NavigateCommand<ProductDetailsViewModel>(productDetailsViewNavigationService);
		NavigateToAddProductCommand = new NavigateCommand<AddProductViewModel>(addProductViewNavigationService);
	}

	public static FactoryDetailsViewModel LoadViewModel(
		int factoryId,
		NavigationStore navigationStore,
		FactoryStore factoryStore,
		ProductStore productStore,
		NavigationService<ProductDetailsViewModel> productDetailsViewNavigationService,
		NavigationService<AddProductViewModel> addProductViewNavigationService,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		var viewModel = new FactoryDetailsViewModel(
			factoryId,
			navigationStore,
			factoryStore,
			productStore,
			productDetailsViewNavigationService, 
			addProductViewNavigationService,
			snackbarMessageQueue);
		viewModel.Load();
		return viewModel;
	}

	public async void Load()
	{
		await LoadFactoryByIdCommand.ExecuteAsync(_factoryId);
		await LoadProductsCommand.ExecuteAsync(null);
	}

	private FactoryListItemViewModel _factory = null!;
	private FactoryListItemViewModel _tempFactory = null!;
	public FactoryListItemViewModel Factory
	{
		get => _factory;
		set => SetField(ref _factory, value);
	}

	private bool _isLoading = true;
	public bool IsLoading
	{
		get => _isLoading;
		set => SetField(ref _isLoading, value);
	}

	private bool _isEditing;
	public bool IsEditing
	{
		get => _isEditing;
		set => SetField(ref _isEditing, value);
	}

	public AsyncCommandBase LoadFactoryByIdCommand { get; }
	public ICommand NavigateBackCommand { get; }
	public ICommand EditCommand { get; }
	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
	public ICommand DeleteCommand { get; }

	private void OnEditExecuted(object? p)
	{
		IsEditing = true;
		_tempFactory = new FactoryListItemViewModel(Factory.Factory);
	}

	private void OnCancelExecuted(object? p)
	{
		IsEditing = false;
		Factory = _tempFactory;
	}

	public void UpdateFactory(Domain.Models.Factory factory)
	{
		Factory = new FactoryListItemViewModel(factory);
		IsLoading = false;
	}

	private readonly ObservableCollection<ProductListItemViewModel> _products;
	public IEnumerable<ProductListItemViewModel> Products => _products;

	public AsyncCommandBase LoadProductsCommand { get; }
	public ICommand NavigateToProductDetailsCommand { get; }
	public ICommand NavigateToAddProductCommand { get; }

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
	}

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
}