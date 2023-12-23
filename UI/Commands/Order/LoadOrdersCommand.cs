using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Customer;

namespace UI.Commands.Order;

public class LoadOrdersCommand : AsyncCommandBase
{
	private readonly CustomerDetailsViewModel _customerDetailsViewModel;
	private readonly OrderStore _orderStore;
	private readonly int _customerId;

	public LoadOrdersCommand(CustomerDetailsViewModel customerDetailsViewModel, OrderStore orderStore, int customerId)
	{
		_customerDetailsViewModel = customerDetailsViewModel;
		_orderStore = orderStore;
		_customerId = customerId;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var orders = await _orderStore.GetOrdersOfSpecificCustomer(_customerId);
			_customerDetailsViewModel.UpdateOrders(orders);
		}
		catch (Exception)
		{
			throw;
		}
	}
}