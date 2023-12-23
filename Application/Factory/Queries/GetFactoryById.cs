using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Factory.Queries;

public class GetFactoryByIdQuery : IRequest<Domain.Models.Factory?>
{
	public int Id { get; set; }
}

public class GetFactoryByIdQueryHandler : IRequestHandler<GetFactoryByIdQuery, Domain.Models.Factory?>
{
	private readonly IFactoryRepository _factoryRepository;
	private readonly IAddressRepository _addressRepository;

	public GetFactoryByIdQueryHandler(IFactoryRepository factoryRepository, IAddressRepository addressRepository)
	{
		_factoryRepository = factoryRepository;
		_addressRepository = addressRepository;
	}

	public async Task<Domain.Models.Factory?> Handle(GetFactoryByIdQuery request, CancellationToken cancellationToken)
	{
		var factory =  await _factoryRepository.FindByIdAsync(request.Id);

		if (factory == null) return null;

		var factoryAddress = await _addressRepository.FindByIdAsync(factory.Id);

		factory.Address = factoryAddress!;

		return factory;
	}
}