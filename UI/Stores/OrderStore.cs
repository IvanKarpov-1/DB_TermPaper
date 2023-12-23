using Application.Order.Commands;
using Application.Order.Queries;
using Domain.Models;
using MediatR;

namespace UI.Stores;

public class OrderStore
{
	private readonly IMediator _mediator;
	//private readonly HashSet<Order> _orders;
	//private readonly Dictionary<int, List<Order?>> _customerOrders;

	//public IEnumerable<Order?> OrdersFromCustomer(int customerId)
	//{
	//	if (_customerOrders.ContainsKey(customerId) == false)
	//		_customerOrders[customerId] = new List<Order?>();
	//	return _customerOrders[customerId];
	//}

	//public IEnumerable<Order> Orders => _orders;

	//public event Action<Order>? OrderAdded;
	//public event Action<int>? OrderRemoved;

	public OrderStore(IMediator mediator)
	{
		_mediator = mediator;
		//_orders = new HashSet<Order>();
		//_customerOrders = new Dictionary<int, List<Order?>>();
	}

	public async Task<Order?> GetById(int orderId)
	{
		return await _mediator.Send(new GetOrderByIdQuery { Id = orderId });
	}

	public async Task<IEnumerable<Order?>> GetOrdersOfSpecificCustomer(int customerId)
	{
		//if (_customerOrders.ContainsKey(customerId)) return;

		var orders = (await _mediator.Send(new GetCustomerOrdersQuery { Id = customerId })).ToList();
		return orders;

		//_customerOrders[customerId] = orders;

		////if (_customerOrders.TryGetValue(customerId, out var value))
		////	value.Clear();
		////else
		////	_customerOrders[customerId] = new List<Order?>();

		////_customerOrders[customerId].AddRange(orders);

		//foreach (var order in orders)
		//{
		//	_orders.Add(order);
		//}

		//return orders;
	}

	public async Task Add(Order order)
	{
		var orderId = await _mediator.Send(new AddOrderCommand { Order = order });

		//if (orderId != null) order.Id = (int)orderId;

		//_orders.Add(order);

		//if (_customerOrders.ContainsKey((int)order.CustomerId!) == false)
		//	_customerOrders[(int)order.CustomerId!] = new List<Order?>();
		//_customerOrders[(int)order.CustomerId!].Add(order);

		//OnAddOrder(order);
	}

	public async Task Update(Order order)
	{
		await _mediator.Send(new UpdateOrderCommand { Order = order });

		//_orders.RemoveWhere(x => x.Id == order.Id);
		//_orders.Add(order);

		//if (_customerOrders.ContainsKey((int)order.CustomerId!) == false)
		//{
		//	_customerOrders[(int)order.CustomerId!] = new List<Order?>();
		//}
		//else
		//{
		//	_customerOrders[(int)order.CustomerId!]
		//		.RemoveAt(_customerOrders[(int)order.CustomerId!].FindIndex(x => x.Id == order.Id));
		//	_customerOrders[(int)order.CustomerId!].Add(order);
		//}

		//OnAddOrder(order);
	}

	public async Task Delete(int orderId, int customerId)
	{
		await _mediator.Send(new DeleteOrderCommand { Id = orderId });

		//_orders.RemoveWhere(x => x.Id == orderId);

		//if (_customerOrders.ContainsKey(customerId))
		//	_customerOrders[customerId].RemoveAt(_customerOrders[customerId].FindIndex(x => x.Id == orderId));

		//OrderRemoved?.Invoke(orderId);
	}

	//private void OnAddOrder(Order order) => OrderAdded?.Invoke(order);
}