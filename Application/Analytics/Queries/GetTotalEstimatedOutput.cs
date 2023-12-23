using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Analytics.Queries;

public class GetTotalEstimatedOutputQuery : IRequest<decimal>
{ }

public class GetTotalEstimatedOutputQueryHandler : IRequestHandler<GetTotalEstimatedOutputQuery, decimal>
{
	private readonly IAnalyticsRepository _analyticsRepository;

	public GetTotalEstimatedOutputQueryHandler(IAnalyticsRepository analyticsRepository)
	{
		_analyticsRepository = analyticsRepository;
	}

	public async Task<decimal> Handle(GetTotalEstimatedOutputQuery request, CancellationToken cancellationToken)
	{
		return await _analyticsRepository.GetTotalEstimatedOutput();
	}
}