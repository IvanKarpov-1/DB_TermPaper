using Application.Abstractions.Repositories;
using Domain.Models;
using MediatR;

namespace Application.Customer.Commands;

public class AddCustomerCommand : IRequest<int?>
{
	public Domain.Models.Customer? Customer { get; set; }
	public Domain.Models.Address? Address { get; set; }
}

public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, int?>
{
	private readonly IAddressRepository _addressRepository;
	private readonly ICustomerRepository _customerRepository;
	private readonly ICustomerAddressRepository _customerAddressRepository;

	public AddCustomerCommandHandler(IAddressRepository addressRepository, ICustomerRepository customerRepository, ICustomerAddressRepository customerAddressRepository)
	{
		_addressRepository = addressRepository;
		_customerRepository = customerRepository;
		_customerAddressRepository = customerAddressRepository;
	}

	public async Task<int?> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
	{
		if (request.Address == null) return null;
		if (request.Customer == null) return null;

		var addressId = await _addressRepository.AddAsync(request.Address);
		var customerId = await _customerRepository.AddAsync(request.Customer);
		
		await _customerAddressRepository.AddAsync(new CustomerAddress
		{
			AddressId = addressId,
			CustomerId = customerId,
		});
		
		return customerId;
	}
}