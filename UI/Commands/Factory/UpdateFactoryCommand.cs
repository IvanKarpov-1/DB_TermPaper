using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Factory;

namespace UI.Commands.Factory;

public class UpdateFactoryCommand : AsyncCommandBase
{
	private readonly FactoryDetailsViewModel _factoryDetailsViewModel;
	private readonly FactoryStore _factoryStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public UpdateFactoryCommand(FactoryDetailsViewModel factoryDetailsViewModel, FactoryStore factoryStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_factoryDetailsViewModel = factoryDetailsViewModel;
		_factoryStore = factoryStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			_factoryDetailsViewModel.IsEditing = false;
			await _factoryStore.Update(_factoryDetailsViewModel.Factory.Factory);
			_snackbarMessageQueue.Enqueue("Інформація про завод успісшно змінена");
		}
		catch (Exception)
		{
			// ignored
		}
	}
}