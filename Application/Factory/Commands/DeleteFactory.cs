using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Factory.Commands;

public class DeleteFactoryCommand : IRequest<Unit>
{
	public int Id { get; set; }
}

public class DeleteFactoryCommandHandler : IRequestHandler<DeleteFactoryCommand, Unit>
{
	private readonly IFactoryRepository _factoryRepository;

	public DeleteFactoryCommandHandler(IFactoryRepository factoryRepository)
	{
		_factoryRepository = factoryRepository;
	}

	public async Task<Unit> Handle(DeleteFactoryCommand request, CancellationToken cancellationToken)
	{
		await _factoryRepository.DeleteAsync(request.Id);
		return Unit.Value;
	}
}