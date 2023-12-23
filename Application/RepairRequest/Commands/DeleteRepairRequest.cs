using Application.Abstractions.Repositories;
using MediatR;

namespace Application.RepairRequest.Commands;

public class DeleteRepairRequestCommand : IRequest<Unit>
{
	public int Id { get; set; }
}

public class DeleteRepairRequestCommandHandler : IRequestHandler<DeleteRepairRequestCommand, Unit>
{
	private readonly IRepairRequestRepository _repairRequestRepository;

	public DeleteRepairRequestCommandHandler(IRepairRequestRepository repairRequestRepository)
	{
		_repairRequestRepository = repairRequestRepository;
	}

	public async Task<Unit> Handle(DeleteRepairRequestCommand request, CancellationToken cancellationToken)
	{
		await _repairRequestRepository.DeleteAsync(request.Id);
		return Unit.Value;
	}
}