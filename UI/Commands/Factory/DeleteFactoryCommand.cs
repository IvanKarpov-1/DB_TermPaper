using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Factory;

namespace UI.Commands.Factory;

public class DeleteFactoryCommand : AsyncCommandBase
{
	private readonly FactoryDetailsViewModel _factoryDetailsViewModel;
	private readonly FactoryStore _factoryStore;
	private readonly NavigationStore _navigationStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public DeleteFactoryCommand(FactoryDetailsViewModel factoryDetailsViewModel, FactoryStore factoryStore, NavigationStore navigationStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_factoryDetailsViewModel = factoryDetailsViewModel;
		_factoryStore = factoryStore;
		_navigationStore = navigationStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _factoryStore.Delete(_factoryDetailsViewModel.Factory.Id);
			_factoryDetailsViewModel.NavigateBackCommand.Execute(parameter);
			_navigationStore.ClearForwardHistory();
			_snackbarMessageQueue.Enqueue("Завод успішно видалено");
		}
		catch (Exception)
		{
			// ignored
		}
	}
}