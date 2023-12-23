namespace UI.ViewModels.Base;

public interface INeedCustomerAddressesViewModel
{
	public void UpdateAddresses(IEnumerable<Domain.Models.Address> addresses);
}