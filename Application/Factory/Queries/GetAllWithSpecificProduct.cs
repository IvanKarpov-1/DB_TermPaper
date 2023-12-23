using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Factory.Queries;

public class GetAllWithSpecificProductQuery : IRequest<IEnumerable<Domain.Models.Factory>>
{
	public int ProductId { get; set; }
}

public class GetAllWithSpecificProductQueryHandler : IRequestHandler<GetAllWithSpecificProductQuery, IEnumerable<Domain.Models.Factory>>
{
	private readonly IFactoryRepository _addressRepository;

	public GetAllWithSpecificProductQueryHandler(IFactoryRepository addressRepository)
	{
		_addressRepository = addressRepository;
	}

	public async Task<IEnumerable<Domain.Models.Factory>> Handle(GetAllWithSpecificProductQuery request, CancellationToken cancellationToken)
	{
		return await _addressRepository.GetAllWithSpecificProductAsync(request.ProductId);
	}
}