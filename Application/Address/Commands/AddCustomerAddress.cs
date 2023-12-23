using Application.Abstractions.Repositories;
using Domain.Models;
using MediatR;

namespace Application.Address.Commands;

public class AddCustomerAddressCommand : IRequest<int?>
{
	public Domain.Models.Address? Address { get; set; }	
	public int CustomerId { get; set; }
}

public class AddCustomerAddressCommandHandler : IRequestHandler<AddCustomerAddressCommand, int?>
{
	private readonly IAddressRepository _addressRepository;
	private readonly ICustomerAddressRepository _customerAddressRepository;

	public AddCustomerAddressCommandHandler(IAddressRepository addressRepository, ICustomerAddressRepository customerAddressRepository)
	{
		_addressRepository = addressRepository;
		_customerAddressRepository = customerAddressRepository;
	}

	public async Task<int?> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
	{
		if (request.Address == null) return null;

		var addressId = await _addressRepository.AddAsync(request.Address);

		await _customerAddressRepository.AddAsync(new CustomerAddress
		{
			AddressId = addressId,
			CustomerId = request.CustomerId
		});

		return addressId;
	}
}
