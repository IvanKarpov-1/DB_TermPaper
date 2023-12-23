using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Customer;

namespace UI.Commands.Customer;

public class DeleteCustomerCommand : AsyncCommandBase
{
	private readonly CustomerDetailsViewModel _customerDetailsViewModel;
	private readonly CustomerStore _customerStore;
	private readonly NavigationStore _navigationStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public DeleteCustomerCommand(CustomerDetailsViewModel customerDetailsViewModel, CustomerStore customerStore, NavigationStore navigationStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_customerDetailsViewModel = customerDetailsViewModel;
		_customerStore = customerStore;
		_navigationStore = navigationStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{

		}
		catch (Exception)
		{
			throw;
		}
	}
}