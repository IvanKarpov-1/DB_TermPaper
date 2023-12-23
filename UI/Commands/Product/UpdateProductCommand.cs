using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Product;

namespace UI.Commands.Product;

public class UpdateProductCommand : AsyncCommandBase
{
	private readonly ProductDetailsViewModel _productDetailsViewModel;
	private readonly ProductStore _productStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public UpdateProductCommand(ProductDetailsViewModel productDetailsViewModel, ProductStore productStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_productDetailsViewModel = productDetailsViewModel;
		_productStore = productStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			_productDetailsViewModel.IsEditing = false;
			await _productStore.Update(_productDetailsViewModel.Product.Product);
			_snackbarMessageQueue.Enqueue("Інформація про товар успісшно змінена");
		}
		catch (Exception)
		{
			throw;
		}
	}
}