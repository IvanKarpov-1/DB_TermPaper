using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Base;
using UI.Commands.Product;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.ViewModels.Product;

public class ProductDetailsViewModel : ViewModelBase, ILoadingViewModel, IEditableViewModel
{
	private readonly int _productId;

	public ProductDetailsViewModel(
		int productId,
		ProductStore productStore,
		NavigationStore navigationStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_productId = productId;
		LoadProductByIdCommand = new LoadProductByIdCommand(this, productStore);
		NavigateBackCommand = new NavigateBackCommand(navigationStore);
		EditCommand = new RelayCommand(OnEditExecuted, _ => true);
		SaveCommand = new UpdateProductCommand(this, productStore, snackbarMessageQueue);
		CancelCommand = new RelayCommand(OnCancelExecuted, _ => true);
		DeleteCommand = new DeleteProductCommand(this, productStore, navigationStore, snackbarMessageQueue);
	}

	public static ProductDetailsViewModel LoadViewModel(
		int productId,
		NavigationStore navigationStore,
		ProductStore productStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		var viewModel = new ProductDetailsViewModel(
			productId,
			productStore,
			navigationStore,
			snackbarMessageQueue);
		viewModel.Load();
		return viewModel;
	}

	public async void Load()
	{
		await LoadProductByIdCommand.ExecuteAsync(_productId);
	}

	private ProductListItemViewModel _product = null!;
	private ProductListItemViewModel _tempProduct = null!;
	public ProductListItemViewModel Product
	{
		get => _product;
		set => SetField(ref _product, value);
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

	public AsyncCommandBase LoadProductByIdCommand { get; }
	public ICommand NavigateBackCommand { get; }
	public ICommand EditCommand { get; }
	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
	public ICommand DeleteCommand { get; }

	private void OnEditExecuted(object? p)
	{
		IsEditing = true;
		_tempProduct = new ProductListItemViewModel(Product.GetProduct());
	}

	private void OnCancelExecuted(object? p)
	{
		IsEditing = false;
		Product = _tempProduct;
	}

	public void UpdateProduct(Domain.Models.Product product)
	{
		Product = new ProductListItemViewModel(product);
		IsLoading = false;
	}
}