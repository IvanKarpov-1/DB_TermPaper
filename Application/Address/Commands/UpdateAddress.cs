using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Address.Commands;

public class UpdateAddressCommand : IRequest<Unit>
{
	public Domain.Models.Address? Address { get; set; }
}

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Unit>
{
	private readonly IAddressRepository _addressRepository;

	public UpdateAddressCommandHandler(IAddressRepository addressRepository)
	{
		_addressRepository = addressRepository;
	}

	public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
	{
		if (request.Address != null) await _addressRepository.UpdateAsync(request.Address);
		return Unit.Value;
	}
}