using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Factory.Commands;

public class AddFactoryCommand : IRequest<int?>
{
	public Domain.Models.Factory? Factory { get; set; }
	public Domain.Models.Address? Address { get; set; }
}

public class AddFactoryCommandHandler : IRequestHandler<AddFactoryCommand, int?>
{
	private readonly IAddressRepository _addressRepository;
	private readonly IFactoryRepository _factoryRepository;

	public AddFactoryCommandHandler(IAddressRepository addressRepository, IFactoryRepository factoryRepository)
	{
		_addressRepository = addressRepository;
		_factoryRepository = factoryRepository;
	}

	public async Task<int?> Handle(AddFactoryCommand request, CancellationToken cancellationToken)
	{
		if (request.Factory == null) return null;
		if (request.Address == null) return null;

		await _addressRepository.AddAsync(request.Address);

		var address = await _addressRepository.GetLastAdded();

		request.Factory.AddressId = address!.Id;

	 	var factoryId = await _factoryRepository.AddAsync(request.Factory);

		return factoryId;
	}
}