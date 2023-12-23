using System.Printing;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Customer;

namespace UI.Commands.Customer;

public class LoadCustomerByIdCommand : AsyncCommandBase
{
	private readonly CustomerDetailsViewModel _customerDetailsViewModel;
	private readonly CustomerStore _customerStore;

	public LoadCustomerByIdCommand(CustomerDetailsViewModel customerDetailsViewModel, CustomerStore customerStore)
	{
		_customerDetailsViewModel = customerDetailsViewModel;
		_customerStore = customerStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			int.TryParse(parameter?.ToString(), out var id);
			var customer = await _customerStore.GetById(id);
			if (customer != null) _customerDetailsViewModel.UpdateCustomer(customer);
		}
		catch (Exception)
		{
			throw;
		}
	}
}