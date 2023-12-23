using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Address.Commands;

public class DeleteAddressCommand : IRequest<Unit>
{
	public int Id { get; set; }
}

public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Unit>
{
	private readonly IAddressRepository _addressRepository;

	public DeleteAddressCommandHandler(IAddressRepository addressRepository)
	{
		_addressRepository = addressRepository;
	}

	public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
	{
		await _addressRepository.DeleteAsync(request.Id);
		return Unit.Value;
	}
}