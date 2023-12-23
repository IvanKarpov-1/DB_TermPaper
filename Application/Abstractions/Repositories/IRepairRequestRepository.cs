namespace Application.Abstractions.Repositories;

public interface IRepairRequestRepository
{
	public Task<Domain.Models.RepairRequest?> FindByIdAsync(int repairRequestId);
	public Task<IEnumerable<Domain.Models.RepairRequest>> GetAllCustomerRepairRequestsAsync(int customerId);
	public Task<int> AddAsync(Domain.Models.RepairRequest repairRequest);
	public Task DeleteAsync(int repairRequestId);
	public Task UpdateAsync(Domain.Models.RepairRequest repairRequest);
}