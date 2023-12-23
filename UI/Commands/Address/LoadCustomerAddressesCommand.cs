using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.Commands.Address;

public class LoadCustomerAddressesCommand : AsyncCommandBase
{
	private readonly INeedCustomerAddressesViewModel _needCustomerAddressesViewModel;
	private readonly AddressStore _addressStore;
	private readonly int _customerId;

	public LoadCustomerAddressesCommand(INeedCustomerAddressesViewModel needCustomerAddressesViewModel, AddressStore addressStore, int customerId)
	{
		_needCustomerAddressesViewModel = needCustomerAddressesViewModel;
		_addressStore = addressStore;
		_customerId = customerId;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var addresses = await _addressStore.GetAddressesOfSpecificCustomer(_customerId);
			_needCustomerAddressesViewModel.UpdateAddresses(addresses);
		}
		catch (Exception)
		{
			throw;
		}	
	}
}