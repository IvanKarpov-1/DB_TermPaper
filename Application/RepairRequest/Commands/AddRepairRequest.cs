using Application.Abstractions.Repositories;
using MediatR;

namespace Application.RepairRequest.Commands;

public class AddRepairRequestCommand : IRequest<int?>
{
	public Domain.Models.RepairRequest? RepairRequest { get; set; }
}

public class AddRepairRequestCommandHandler: IRequestHandler<AddRepairRequestCommand, int?>
{
	private readonly IRepairRequestRepository _repairRequestRepository;

	public AddRepairRequestCommandHandler(IRepairRequestRepository repairRequestRepository)
	{
		_repairRequestRepository = repairRequestRepository;
	}

	public async Task<int?> Handle(AddRepairRequestCommand request, CancellationToken cancellationToken)
	{
		if (request.RepairRequest == null) return null;
		var repairRequestId = await _repairRequestRepository.AddAsync(request.RepairRequest);
		return repairRequestId;
	}
}