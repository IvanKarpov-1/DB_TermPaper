using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Address.Queries;

public class GetAddressByIdQuery : IRequest<Domain.Models.Address?>
{
	public int Id { get; set; }
}

public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, Domain.Models.Address?>
{
	private readonly IAddressRepository _addressRepository;

	public GetAddressByIdQueryHandler(IAddressRepository addressRepository)
	{
		_addressRepository = addressRepository;
	}

	public async Task<Domain.Models.Address?> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
	{
		return await _addressRepository.FindByIdAsync(request.Id);
	}
}