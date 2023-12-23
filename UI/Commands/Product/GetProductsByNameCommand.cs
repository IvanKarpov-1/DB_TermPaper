using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Order;

namespace UI.Commands.Product;

public class GetProductsByNameCommand : AsyncCommandBase
{
	private readonly AddOrderViewModel _addOrderViewModel;
	private readonly ProductStore _productStore;

	public GetProductsByNameCommand(AddOrderViewModel addOrderViewModel, ProductStore productStore)
	{
		_addOrderViewModel = addOrderViewModel;
		_productStore = productStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var products = await _productStore.GetByName(_addOrderViewModel.ProductName);
			_addOrderViewModel.UpdateMatchedProducts(products);
		}
		catch (Exception)
		{
			throw;
		}
	}
}