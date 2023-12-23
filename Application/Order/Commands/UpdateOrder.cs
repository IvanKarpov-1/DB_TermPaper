using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Order.Commands;

public class UpdateOrderCommand : IRequest<Unit>
{
	public Domain.Models.Order? Order { get; set; }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
{
	private readonly IOrderRepository _orderRepository;

	public UpdateOrderCommandHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
	{
		if (request.Order != null) await _orderRepository.UpdateAsync(request.Order);
		return Unit.Value;
	}
}