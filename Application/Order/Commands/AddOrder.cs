using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Order.Commands;

public class AddOrderCommand : IRequest<int?>
{
	public Domain.Models.Order? Order { get; set; }
}

public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, int?>
{
	private readonly IOrderRepository _orderRepository;

	public AddOrderCommandHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<int?> Handle(AddOrderCommand request, CancellationToken cancellationToken)
	{
		if (request.Order == null) return null;
		var orderId = await _orderRepository.AddAsync(request.Order);
		return orderId;
	}
}