using Application.Abstractions.Repositories;
using MediatR;

namespace Application.RepairRequest.Commands;

public class UpdateRepairRequestCommand : IRequest<Unit>
{
	public Domain.Models.RepairRequest? RepairRequest { get; set; }
}

public class UpdateRepairRequestCommandHandler : IRequestHandler<UpdateRepairRequestCommand, Unit>
{
	private readonly IRepairRequestRepository _repairRequestRepository;

	public UpdateRepairRequestCommandHandler(IRepairRequestRepository repairRequestRepository)
	{
		_repairRequestRepository = repairRequestRepository;
	}

	public async Task<Unit> Handle(UpdateRepairRequestCommand request, CancellationToken cancellationToken)
	{
		if (request.RepairRequest != null) await _repairRequestRepository.UpdateAsync(request.RepairRequest);
		return Unit.Value;
	}
}