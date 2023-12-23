using Application.Abstractions.Repositories;
using Application.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Persistence.DbContext;

namespace Persistence.Repositories;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
	private readonly ICompanyDbContextFactory _companyDbContextFactory;

	protected GenericRepository(ICompanyDbContextFactory companyDbContextFactory)
	{
		_companyDbContextFactory = companyDbContextFactory;
	}

	public async Task<TEntity?> FindEntityByColumnAsync(string tableName, string columnName, object columnValue)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var columnParameter = new NpgsqlParameter("columnValue", columnValue);

		var sqlQuery = $@"
					SELECT
						*
					FROM
						{tableName}
					WHERE
						{columnName} = @columnValue";

		var dbEntity = await context
			.GetEntityDbSet<TEntity>()
			.FromSqlRaw(sqlQuery, columnParameter)
			.FirstOrDefaultAsync();

		return dbEntity;
	}

	public async Task<TEntity?> GetLastAddedEntity(string tableName)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var sqlQuery = $@"
					SELECT
						*
					FROM
						{tableName}
					ORDER BY
						id DESC";

		var dbEntity = await context
			.GetEntityDbSet<TEntity>()
			.FromSqlRaw(sqlQuery)
			.FirstOrDefaultAsync();

		return dbEntity;
	}

	public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync(string tableName, PagingParams pagingParams)
	{
		//await Task.Delay(3000);

		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var sqlQuery = $@"
							SELECT
								*
							FROM
								{tableName}
							ORDER BY
								id
							LIMIT
								{pagingParams.PageSize}
							OFFSET
								{(pagingParams.PageNumber - 1) * pagingParams.PageSize}";

		var dbEntities = await context
			.GetEntityDbSet<TEntity>()
			.FromSqlRaw(sqlQuery)
			.ToListAsync();
				
		return dbEntities;
	}

	public async Task DeleteEntityAsync(string tableName, int entityId)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var idParameter = new NpgsqlParameter("id", entityId);

		var sqlQuery = $@"
					DELETE FROM {tableName}
					WHERE id = @id";

		await context
			.Database
			.ExecuteSqlRawAsync(sqlQuery, idParameter);
	}
}