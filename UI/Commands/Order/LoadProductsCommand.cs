using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Order;

namespace UI.Commands.Order;

public class LoadProductsCommand : AsyncCommandBase
{
	private readonly OrderDetailsViewModel _orderDetailsViewModel;
	private readonly ProductStore _productStore;

	public LoadProductsCommand(OrderDetailsViewModel orderDetailsViewModel, ProductStore productStore)
	{
		_orderDetailsViewModel = orderDetailsViewModel;
		_productStore = productStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var order = _orderDetailsViewModel.Order.Order;

			var products = await _productStore.GetProductsFromSpecificOrder(order.Id);

			var orderDetails = order.OrderDetails.Select(orderDetail => (
				products.FirstOrDefault(p => p.Id == orderDetail.ProductId),
				orderDetail.Quantity)).ToList();
			_orderDetailsViewModel.UpdateOrderDetails(orderDetails);
		}
		catch (Exception)
		{
			throw;
		}
	}
}