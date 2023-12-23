using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Order;

namespace UI.Commands.Order;

public class UpdateOrderCommand : AsyncCommandBase
{
	private readonly OrderDetailsViewModel _orderDetailsViewModel;
	private readonly OrderStore _orderStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public UpdateOrderCommand(OrderDetailsViewModel orderDetailsViewModel, OrderStore orderStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_orderDetailsViewModel = orderDetailsViewModel;
		_orderStore = orderStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			_orderDetailsViewModel.IsEditing = false;
			_orderDetailsViewModel.Order.Order.AddressId = _orderDetailsViewModel.SelectedAddress.Id;
			await _orderStore.Update(_orderDetailsViewModel.Order.Order);
			_snackbarMessageQueue.Enqueue("Замовлення успісшно змінено");
		}
		catch (Exception)
		{
			throw;
		}
	}
}