using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Analytics.Queries;

public class GetNumberOfOrdersQuery : IRequest<int>
{ }

public class GetNumberOfOrdersQueryHandler : IRequestHandler<GetNumberOfOrdersQuery, int>
{
	private readonly IAnalyticsRepository _analyticsRepository;

	public GetNumberOfOrdersQueryHandler(IAnalyticsRepository analyticsRepository)
	{
		_analyticsRepository = analyticsRepository;
	}

	public async Task<int> Handle(GetNumberOfOrdersQuery request, CancellationToken cancellationToken)
	{
		return await _analyticsRepository.GetNumberOfOrders();
	}
}