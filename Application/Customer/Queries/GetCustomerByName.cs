using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Customer.Queries;

public class GetCustomerByNameQuery : IRequest<Domain.Models.Customer?>
{
	public string Name { get; set; } = " ";
}

public class GetCustomerByNameQueryHandler : IRequestHandler<GetCustomerByNameQuery, Domain.Models.Customer?>
{
	private readonly ICustomerRepository _customerRepository;
	private readonly IAddressRepository _addressRepository;

	public GetCustomerByNameQueryHandler(ICustomerRepository customerRepository, IAddressRepository addressRepository)
	{
		_customerRepository = customerRepository;
		_addressRepository = addressRepository;
	}

	public async Task<Domain.Models.Customer?> Handle(GetCustomerByNameQuery request, CancellationToken cancellationToken)
	{
		var customer = await _customerRepository.FindByNameAsync(request.Name);

		if (customer == null) return null;

		var customerAddresses = await _addressRepository.GetAllCustomerAddressesAsync(customer.Id);

		customer.Address = customerAddresses;

		return customer;
	}
}