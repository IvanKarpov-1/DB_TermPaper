using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Order.Queries;

public class GetCustomerOrdersQuery : IRequest<IEnumerable<Domain.Models.Order?>>
{
	public int Id { get; set; }
}

public class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, IEnumerable<Domain.Models.Order?>>
{
	private readonly IOrderRepository _orderRepository;

	public GetCustomerOrdersQueryHandler(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}

	public async Task<IEnumerable<Domain.Models.Order?>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
	{
		return await _orderRepository.GetCustomerOrdersAsync(request.Id);
	}
}