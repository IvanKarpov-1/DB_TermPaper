using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Product;

namespace UI.Commands.Product;

public class LoadProductByIdCommand : AsyncCommandBase
{
	private readonly ProductDetailsViewModel _productDetailsViewModel;
	private readonly ProductStore _productStore;

	public LoadProductByIdCommand(ProductDetailsViewModel productDetailsViewModel, ProductStore productStore)
	{
		_productDetailsViewModel = productDetailsViewModel;
		_productStore = productStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			int.TryParse(parameter?.ToString(), out var id);
			var product = await _productStore.GetById(id);
			if (product != null) _productDetailsViewModel.UpdateProduct(product);
		}
		catch (Exception)
		{
			throw;
		}
	}
}