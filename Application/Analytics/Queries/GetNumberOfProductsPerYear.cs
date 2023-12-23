using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Analytics.Queries;

public class GetNumberOfProductsPerYearQuery : IRequest<int>
{ }

public class GetNumberOfProductsPerYearQueryHandler : IRequestHandler<GetNumberOfProductsPerYearQuery, int>
{
	private readonly IAnalyticsRepository _analyticsRepository;

	public GetNumberOfProductsPerYearQueryHandler(IAnalyticsRepository analyticsRepository)
	{
		_analyticsRepository = analyticsRepository;
	}

	public async Task<int> Handle(GetNumberOfProductsPerYearQuery request, CancellationToken cancellationToken)
	{
		return await _analyticsRepository.GetNumberOfProductsPerYear();
	}
}