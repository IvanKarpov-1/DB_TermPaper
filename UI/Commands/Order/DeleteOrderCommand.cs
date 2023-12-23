using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Order;

namespace UI.Commands.Order;

public class DeleteOrderCommand : AsyncCommandBase
{
	private readonly OrderDetailsViewModel _orderDetailsViewModel;
	private readonly OrderStore _orderStore;
	private readonly NavigationStore _navigationStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public DeleteOrderCommand(OrderDetailsViewModel orderDetailsViewModel, OrderStore orderStore, NavigationStore navigationStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_orderDetailsViewModel = orderDetailsViewModel;
		_orderStore = orderStore;
		_navigationStore = navigationStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var customerId = _orderDetailsViewModel.CustomerId;
			var orderId = _orderDetailsViewModel.Order.Order.Id;

			await _orderStore.Delete(orderId, customerId);
			_orderDetailsViewModel.NavigateBackCommand.Execute(parameter);
			_navigationStore.ClearForwardHistory();
			_snackbarMessageQueue.Enqueue("Замовлення було успішно видалено");
		}
		catch (Exception)
		{
			throw;
		}
	}
}