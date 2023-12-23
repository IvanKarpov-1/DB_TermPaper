using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Factory.Commands;

public class UpdateFactoryCommand : IRequest<Unit>
{
	public Domain.Models.Factory? Factory { get; set; }
}

public class UpdateFactoryCommandHandler : IRequestHandler<UpdateFactoryCommand, Unit>
{
	private readonly IFactoryRepository _factoryRepository;

	public UpdateFactoryCommandHandler(IFactoryRepository factoryRepository)
	{
		_factoryRepository = factoryRepository;
	}

	public async Task<Unit> Handle(UpdateFactoryCommand request, CancellationToken cancellationToken)
	{
		if (request.Factory != null) await _factoryRepository.UpdateAsync(request.Factory);
		return Unit.Value;
	}
}