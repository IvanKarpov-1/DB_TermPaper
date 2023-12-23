namespace Application.Abstractions.Repositories;

public interface IOrderRepository
{
	public Task<Domain.Models.Order?> FindByIdAsync(int orderId);
	public Task<IEnumerable<Domain.Models.Order?>> GetCustomerOrdersAsync(int customerId);
	public Task<int> AddAsync(Domain.Models.Order order);
	public Task DeleteAsync(int orderId);
	public Task UpdateAsync(Domain.Models.Order order);
}