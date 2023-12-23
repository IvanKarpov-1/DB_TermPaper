using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Customer;

namespace UI.Commands.Customer;

public class LoadCustomersCommand : AsyncCommandBase
{
	private readonly CustomerListViewModel _customerListViewModel;
	private readonly CustomerStore _customerStore;

	public LoadCustomersCommand(CustomerListViewModel customerListViewModel, CustomerStore customerStore)
	{
		_customerListViewModel = customerListViewModel;
		_customerStore = customerStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var customers = await _customerStore.GetAll();
			_customerListViewModel.UpdateCustomers(customers);
		}
		catch (Exception)
		{
			throw;
		}
	}
}