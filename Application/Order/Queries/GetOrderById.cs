using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Order.Queries;

public class GetOrderByIdQuery : IRequest<Domain.Models.Order?>
{
	public int Id { get; set; }
}

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Domain.Models.Order?>
{
	private readonly IOrderRepository _orderRepository;

	public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<Domain.Models.Order?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
	{
		return await _orderRepository.FindByIdAsync(request.Id);
	}
}