using Application.Abstractions.Repositories;
using MediatR;

namespace Application.RepairRequest.Queries;

public class GetAllCustomerRepairRequestsQuery : IRequest<IEnumerable<Domain.Models.RepairRequest>>
{
	public int CustomerId { get; set; }
}

public class GetAllCustomerRepairRequestsQueryHandler : IRequestHandler<GetAllCustomerRepairRequestsQuery, IEnumerable<Domain.Models.RepairRequest>>
{
	private readonly IRepairRequestRepository _repairRequestRepository;

	public GetAllCustomerRepairRequestsQueryHandler(IRepairRequestRepository repairRequestRepository)
	{
		_repairRequestRepository = repairRequestRepository;
	}

	public async Task<IEnumerable<Domain.Models.RepairRequest>> Handle(GetAllCustomerRepairRequestsQuery request, CancellationToken cancellationToken)
	{
		return await _repairRequestRepository.GetAllCustomerRepairRequestsAsync(request.CustomerId);
	}
}