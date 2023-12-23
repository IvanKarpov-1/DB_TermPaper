using Application.Common;

namespace Application.Abstractions.Repositories;

public interface IFactoryRepository
{
	public Task<Domain.Models.Factory?> FindByIdAsync(int factoryId);
	public Task<IEnumerable<Domain.Models.Factory>> GetAllAsync(PagingParams? pagingParams = null);
	public Task<IEnumerable<Domain.Models.Factory>> GetAllWithSpecificProductAsync(int productId, PagingParams? pagingParams = null);
	public Task<int> AddAsync(Domain.Models.Factory factory);
	public Task DeleteAsync(int factoryId);
	public Task UpdateAsync(Domain.Models.Factory factory);
}