using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Customer;

namespace UI.Commands.Customer;

public class UpdateCustomerCommand : AsyncCommandBase
{
	private readonly CustomerDetailsViewModel _customerDetailsViewModel;
	private readonly CustomerStore _customerStore;
	private ISnackbarMessageQueue _snackbarMessageQueue;

	public UpdateCustomerCommand(CustomerDetailsViewModel customerDetailsViewModel, CustomerStore customerStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_customerDetailsViewModel = customerDetailsViewModel;
		_customerStore = customerStore;
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