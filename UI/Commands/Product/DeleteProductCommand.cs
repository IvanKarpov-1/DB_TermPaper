using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Product;

namespace UI.Commands.Product;

public class DeleteProductCommand : AsyncCommandBase
{
	private readonly ProductDetailsViewModel _productDetailsViewModel;
	private readonly ProductStore _productStore;
	private readonly NavigationStore _navigationStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public DeleteProductCommand(ProductDetailsViewModel productDetailsViewModel, ProductStore productStore, NavigationStore navigationStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_productDetailsViewModel = productDetailsViewModel;
		_productStore = productStore;
		_navigationStore = navigationStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _productStore.Delete(_productDetailsViewModel.Product.Id);
			_productDetailsViewModel.NavigateBackCommand.Execute(parameter);
			_navigationStore.ClearForwardHistory();
			_snackbarMessageQueue.Enqueue("Товар було успішно видалено");
		}
		catch (Exception)
		{
			throw;
		}
	}
}