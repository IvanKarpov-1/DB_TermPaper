using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Customer;

namespace UI.Commands.Customer;

public class AddCustomerCommand : AsyncCommandBase
{
	private readonly AddCustomerViewModel _addCustomerViewModel;
	private readonly CustomerStore _customerStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public AddCustomerCommand(AddCustomerViewModel addCustomerViewModel, CustomerStore customerStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_addCustomerViewModel = addCustomerViewModel;
		_customerStore = customerStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _customerStore.Add(_addCustomerViewModel.Customer.Customer,
				_addCustomerViewModel.CustomerAddress.Address);
			_addCustomerViewModel.CancelCommand.Execute(parameter);
			_snackbarMessageQueue.Enqueue("Користувача успішно додано");
		}
		catch (Exception)
		{
			throw;
		}
	}
}