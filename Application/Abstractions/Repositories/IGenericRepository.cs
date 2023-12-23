using Application.Common;

namespace Application.Abstractions.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
	public Task<TEntity?> FindEntityByColumnAsync(string tableName, string columnName, object columnValue);
	public Task<TEntity?> GetLastAddedEntity(string tableName);
	public Task<IEnumerable<TEntity>> GetAllEntitiesAsync(string tableName, PagingParams pagingParams);
	public Task DeleteEntityAsync(string tableName, int entityId);
}