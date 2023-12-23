using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Order;

namespace UI.Commands.Order;

public class AddOrderCommand : AsyncCommandBase
{
	private readonly AddOrderViewModel _addOrderViewModel;
	private readonly OrderStore _orderStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;
	
	public AddOrderCommand(AddOrderViewModel addOrderViewModel, OrderStore orderStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_addOrderViewModel = addOrderViewModel;
		_orderStore = orderStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _orderStore.Add(_addOrderViewModel.GetOrder());
			_addOrderViewModel.CancelCommand.Execute(parameter);
			_snackbarMessageQueue.Enqueue("Замовлення успішно додано");
		}
		catch (Exception)
		{
			throw;
		}
	}
}