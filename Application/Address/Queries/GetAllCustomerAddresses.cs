using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Address.Queries;

public class GetAllCustomerAddressesQuery : IRequest<IEnumerable<Domain.Models.Address>>
{
	public int CustomerId { get; set; }
}

public class GetAllCustomerAddressesQueryHandler : IRequestHandler<GetAllCustomerAddressesQuery, IEnumerable<Domain.Models.Address>>
{
	private readonly IAddressRepository _addressRepository;

	public GetAllCustomerAddressesQueryHandler(IAddressRepository addressRepository)
	{
		_addressRepository = addressRepository;
	}

	public async Task<IEnumerable<Domain.Models.Address>> Handle(GetAllCustomerAddressesQuery request, CancellationToken cancellationToken)
	{
		return await _addressRepository.GetAllCustomerAddressesAsync(request.CustomerId);
	}
}