using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Customer.Queries;

public class GetCustomerByIdQuery : IRequest<Domain.Models.Customer?>
{
	public int Id { get; set; }
}

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Domain.Models.Customer?>
{
	private readonly ICustomerRepository _customerRepository;
	private readonly IAddressRepository _addressRepository;

	public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IAddressRepository addressRepository)
	{
		_customerRepository = customerRepository;
		_addressRepository = addressRepository;
	}

	public async Task<Domain.Models.Customer?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
	{
		var customer = await _customerRepository.FindByIdAsync(request.Id);
		
		if (customer == null) return null;
		
		var customerAddresses = await _addressRepository.GetAllCustomerAddressesAsync(customer.Id);
		
		customer.Address = customerAddresses;

		return customer;
	}
}