using Application.Common;

namespace Application.Abstractions.Repositories;

public interface IProductRepository
{
    public Task<Domain.Models.Product?> FindByIdAsync(int productId);
    public Task<IEnumerable<Domain.Models.Product>> FindByNameAsync(string name);
    public Task<IEnumerable<Domain.Models.Product>> GetAllAsync(PagingParams? pagingParams = null);
    public Task<IEnumerable<Domain.Models.Product>> GetAllFromSpecificFactoryAsync(int factoryId, PagingParams? pagingParams = null);
    public Task<IEnumerable<Domain.Models.Product>> GetAllFromSpecificOrderAsync(int orderId);
    public Task<int> AddAsync(Domain.Models.Product product, int factoryId);
    public Task DeleteAsync(int productId);
    public Task UpdateAsync(Domain.Models.Product product);
}