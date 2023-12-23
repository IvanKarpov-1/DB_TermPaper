using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Address;

namespace UI.Commands.Address;

public class LoadAddressByIdCommand : AsyncCommandBase
{
	private readonly AddressDetailsViewModel _addressDetailsViewModel;
	private readonly AddressStore _addressStore;

	public LoadAddressByIdCommand(AddressDetailsViewModel addressDetailsViewModel, AddressStore addressStore)
	{
		_addressDetailsViewModel = addressDetailsViewModel;
		_addressStore = addressStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			int.TryParse(parameter?.ToString(), out var id);
			var address = await _addressStore.GetById(id);
			if (address != null) _addressDetailsViewModel.UpdateAddress(address);
		}
		catch (Exception)
		{
			throw;
		}
	}
}