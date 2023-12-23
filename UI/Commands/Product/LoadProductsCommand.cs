using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Product;

namespace UI.Commands.Product;

public class LoadProductsCommand : AsyncCommandBase
{
	private readonly ProductListViewModel _productListViewModel;
	private readonly ProductStore _productStore;

	public LoadProductsCommand(ProductListViewModel productListViewModel, ProductStore productStore)
	{
		_productListViewModel = productListViewModel;
		_productStore = productStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var products = await _productStore.GetAll();
			_productListViewModel.UpdateProducts(products);
		}
		catch (Exception)
		{
			throw;
		}
	}
}