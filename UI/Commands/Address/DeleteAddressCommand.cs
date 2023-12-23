using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Address;

namespace UI.Commands.Address;

public class DeleteAddressCommand : AsyncCommandBase
{
	private readonly AddressDetailsViewModel _addressDetailsViewModel;
	private readonly AddressStore _addressStore;
	private readonly NavigationStore _navigationStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public DeleteAddressCommand(AddressDetailsViewModel addressDetailsViewModel, AddressStore addressStore, NavigationStore navigationStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_addressDetailsViewModel = addressDetailsViewModel;
		_addressStore = addressStore;
		_navigationStore = navigationStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _addressStore.Delete(_addressDetailsViewModel.Address.Id);
			_addressDetailsViewModel.NavigateBackCommand.Execute(parameter);
			_navigationStore.ClearForwardHistory();
			_snackbarMessageQueue.Enqueue("Адресу успішно видалено");
		}
		catch (Exception)
		{
			throw;
		}
	}
}