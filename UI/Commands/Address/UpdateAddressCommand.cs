using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Address;

namespace UI.Commands.Address;

public class UpdateAddressCommand : AsyncCommandBase
{
	private readonly AddressDetailsViewModel _addressDetailsViewModel;
	private readonly AddressStore _addressStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public UpdateAddressCommand(AddressDetailsViewModel addressDetailsViewModel, AddressStore addressStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_addressDetailsViewModel = addressDetailsViewModel;
		_addressStore = addressStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			_addressDetailsViewModel.IsEditing = false;
			await _addressStore.Update(_addressDetailsViewModel.Address.Address);
			_snackbarMessageQueue.Enqueue("Адресу успішно змінено");
		}
		catch (Exception)
		{
			throw;
		}
	}
}