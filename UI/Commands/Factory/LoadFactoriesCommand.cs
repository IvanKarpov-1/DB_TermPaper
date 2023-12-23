using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.Commands.Factory;

public class LoadFactoriesCommand : AsyncCommandBase
{
	private readonly INeedFactoriesViewModel _needFactoriesViewModel;
	private readonly FactoryStore _factoryStore;

	public LoadFactoriesCommand(INeedFactoriesViewModel needFactoriesViewModel, FactoryStore factoryStore)
	{
		_needFactoriesViewModel = needFactoriesViewModel;
		_factoryStore = factoryStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var factories = await _factoryStore.GetAll();
			_needFactoriesViewModel.UpdateFactories(factories);
		}
		catch (Exception)
		{
			// ignored
		}
	}
}