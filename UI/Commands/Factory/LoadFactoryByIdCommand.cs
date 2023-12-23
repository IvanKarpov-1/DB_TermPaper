using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Factory;

namespace UI.Commands.Factory;

public class LoadFactoryByIdCommand : AsyncCommandBase
{
	private readonly FactoryDetailsViewModel _factoryDetailsViewModel;
	private readonly FactoryStore _factoryStore;

	public LoadFactoryByIdCommand(FactoryDetailsViewModel factoryDetailsViewModel, FactoryStore factoryStore)
	{
		_factoryDetailsViewModel = factoryDetailsViewModel;
		_factoryStore = factoryStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			int.TryParse(parameter?.ToString(), out var id);
			var factory = await _factoryStore.GetById(id);
			if (factory != null) _factoryDetailsViewModel.UpdateFactory(factory);
		}
		catch (Exception)
		{
			throw;
		}
	}
}