using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Address;

namespace UI.Commands.Address;

public class AddAddressCommand : AsyncCommandBase
{
	private readonly AddAddressViewModel _addAddressViewModel;
	private readonly AddressStore _addressStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public AddAddressCommand(AddAddressViewModel addAddressViewModel, AddressStore addressStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_addAddressViewModel = addAddressViewModel;
		_addressStore = addressStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _addressStore.Add(_addAddressViewModel.Address.Address, _addAddressViewModel.CustomerId);
			_addAddressViewModel.CancelCommand.Execute(parameter);
			_snackbarMessageQueue.Enqueue("Адресу було успішно додано");
		}
		catch (Exception)
		{
			throw;
		}
	}
}