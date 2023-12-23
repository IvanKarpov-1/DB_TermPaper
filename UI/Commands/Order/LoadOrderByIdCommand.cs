using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Order;

namespace UI.Commands.Order;

public class LoadOrderByIdCommand : AsyncCommandBase
{
	private readonly OrderDetailsViewModel _orderDetailsViewModel;
	private readonly OrderStore _orderStore;

	public LoadOrderByIdCommand(OrderDetailsViewModel orderDetailsViewModel, OrderStore orderStore)
	{
		_orderDetailsViewModel = orderDetailsViewModel;
		_orderStore = orderStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			int.TryParse(parameter?.ToString(), out var id);
			var order = await _orderStore.GetById(id);
			if (order != null) _orderDetailsViewModel.UpdateOrder(order);
		}
		catch (Exception)
		{
			throw;
		}
	}
}