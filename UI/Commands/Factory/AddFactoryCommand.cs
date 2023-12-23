using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Factory;

namespace UI.Commands.Factory;

public class AddFactoryCommand : AsyncCommandBase
{
	private readonly AddFactoryViewModel _addFactoryViewModel;
	private readonly FactoryStore _factoryStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public AddFactoryCommand(AddFactoryViewModel addFactoryViewModel, FactoryStore factoryStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_addFactoryViewModel = addFactoryViewModel;
		_factoryStore = factoryStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}
	
	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _factoryStore.Add(_addFactoryViewModel.Factory.Factory,
				_addFactoryViewModel.Factory.Factory.Address);
			_addFactoryViewModel.CancelCommand.Execute(parameter);
			_snackbarMessageQueue.Enqueue("Завод успішно додано");
		}
		catch (Exception)
		{
			// ignored
		}
	}
}