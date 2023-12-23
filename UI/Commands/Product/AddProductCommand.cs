using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Product;

namespace UI.Commands.Product;

public class AddProductCommand : AsyncCommandBase
{
	private readonly AddProductViewModel _addProductViewModel;
	private readonly ProductStore _productStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public AddProductCommand(AddProductViewModel addProductViewModel, ProductStore productStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_addProductViewModel = addProductViewModel;
		_productStore = productStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _productStore.Add(_addProductViewModel.Product.Product, _addProductViewModel.SelectedFactory.Id);
			_addProductViewModel.CancelCommand.Execute(parameter);
			_snackbarMessageQueue.Enqueue("Товар успішно додано");
		}
		catch (Exception)
		{
			throw;
		}
	}
}