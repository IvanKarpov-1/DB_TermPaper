using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Factory;

namespace UI.Commands.Factory;

public class LoadProductsCommand : AsyncCommandBase
{
	private readonly FactoryDetailsViewModel _factoryDetailsViewModel;
	private readonly ProductStore _productStore;
	private readonly int _factoryId;

	public LoadProductsCommand(FactoryDetailsViewModel factoryDetailsViewModel, ProductStore productStore, int factoryId)
	{
		_factoryDetailsViewModel = factoryDetailsViewModel;
		_productStore = productStore;
		_factoryId = factoryId;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var products = await _productStore.GetProductsFromSpecificFactory(_factoryId);
			_factoryDetailsViewModel.UpdateProducts(products);
		}
		catch (Exception)
		{
			// ignored
		}
	}
}