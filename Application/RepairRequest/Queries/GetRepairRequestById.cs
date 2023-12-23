using Application.Abstractions.Repositories;
using MediatR;

namespace Application.RepairRequest.Queries;

public class GetRepairRequestByIdQuery : IRequest<Domain.Models.RepairRequest?>
{
	public int Id { get; set; }
}

public class GetRepairRequestByIdQueryHandler : IRequestHandler<GetRepairRequestByIdQuery, Domain.Models.RepairRequest?>
{
	private readonly IRepairRequestRepository _repairRequestRepository;

	public GetRepairRequestByIdQueryHandler(IRepairRequestRepository repairRequestRepository)
	{
		_repairRequestRepository = repairRequestRepository;
	}

	public async Task<Domain.Models.RepairRequest?> Handle(GetRepairRequestByIdQuery request, CancellationToken cancellationToken)
	{
		return await _repairRequestRepository.FindByIdAsync(request.Id);
	}
}