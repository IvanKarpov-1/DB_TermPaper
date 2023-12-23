using Application.Abstractions.Repositories;
using Application.Common;
using MediatR;

namespace Application.Factory.Queries;

public class GetFactoriesQuery : IRequest<IEnumerable<Domain.Models.Factory>>
{
	public PagingParams? PagingParams { get; set; }
}

public class GetFactoriesQueryHandler : IRequestHandler<GetFactoriesQuery, IEnumerable<Domain.Models.Factory>>
{
	private readonly IFactoryRepository _factoryRepository;

	public GetFactoriesQueryHandler(IFactoryRepository factoryRepository)
	{
		_factoryRepository = factoryRepository;
	}

	public async Task<IEnumerable<Domain.Models.Factory>> Handle(GetFactoriesQuery request, CancellationToken cancellationToken)
	{
		var factories = await _factoryRepository.GetAllAsync(request.PagingParams);
		return factories;
	}
}