using Application.Common;

namespace Application.Abstractions.Repositories;

public interface ICustomerRepository
{
	public Task<Domain.Models.Customer?> FindByIdAsync(int customerId);
	public Task<Domain.Models.Customer?> FindByNameAsync(string name);
	public Task<Domain.Models.Customer?> GetLastAdded();
	public Task<IEnumerable<Domain.Models.Customer>> GetAllAsync(PagingParams? pagingParams = null);
	public Task<int> AddAsync(Domain.Models.Customer customer);
	public Task DeleteAsync(int customerId);
	public Task UpdateAsync(Domain.Models.Customer customer);
}