using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Order.Commands;

public class DeleteOrderCommand : IRequest<Unit>
{
	public int Id { get; set; }
}

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
	private readonly IOrderRepository _orderRepository;

	public DeleteOrderCommandHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
	{
		await _orderRepository.DeleteAsync(request.Id);
		return Unit.Value;
	}
}