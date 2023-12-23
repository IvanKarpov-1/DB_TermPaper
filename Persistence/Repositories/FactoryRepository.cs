using Application.Abstractions.Repositories;
using Application.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class FactoryRepository : GenericRepository<DbModels.Factory>, IFactoryRepository
{
	private readonly ICompanyDbContextFactory _companyDbContextFactory;
	private readonly MapperlyMapper _mapper;
	private const string TableName = "factory";

	public FactoryRepository(ICompanyDbContextFactory companyDbContextFactory, MapperlyMapper mapper)
		:base(companyDbContextFactory)
	{
		_companyDbContextFactory = companyDbContextFactory;
		_mapper = mapper;
	}

	public async Task<Factory?> FindByIdAsync(int factoryId)
	{
		var dbFactory = await FindEntityByColumnAsync(TableName, "id", factoryId);

		if (dbFactory == null) return null;

		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var dbAddress = await context
			.Addresses
			.FromSql($@"
					SELECT
					    a.id,
					    a.country,
					    a.region,
					    a.city,
					    a.address_line1,
					    a.address_line2,
					    a.post_code
					FROM address     AS a
					    JOIN factory AS f ON f.address_id = a.id
					WHERE
					    a.id = {dbFactory.AddressId}").FirstOrDefaultAsync();

		var domainFactory = _mapper.Map(dbFactory);
			
		if (dbAddress == null) return domainFactory;
			
		var domainAddress = _mapper.Map(dbAddress);
		domainFactory.Address = domainAddress;

		return domainFactory;
	}

	public async Task<IEnumerable<Factory>> GetAllAsync(PagingParams? pagingParams = null)
	{
		pagingParams ??= PagingParams.Default;

		var dbFactories = (await GetAllEntitiesAsync(TableName, pagingParams)).ToList();

		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var modelFactories = await GetModelFactoriesWithAddresses(dbFactories, context);

		return modelFactories;
	}

	public async Task<IEnumerable<Factory>> GetAllWithSpecificProductAsync(int productId, PagingParams? pagingParams = null)
	{
		pagingParams ??= PagingParams.Default;

		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var dbFactories = await context
			.Factories
			.FromSql($@"
					SELECT
					    f.id,
						f.phone,
						f.email,
						f.address_id
					FROM product AS p
					    JOIN factory_product AS fp ON fp.product_id = p.id
					    JOIN factory         AS f  ON f.id = fp.factory_id
					WHERE
					    p.id = {productId}
					ORDER BY
						f.id
					LIMIT
						{pagingParams.PageSize}
					OFFSET
						{(pagingParams.PageNumber - 1) * pagingParams.PageSize}").ToListAsync();

		var modelFactories = await GetModelFactoriesWithAddresses(dbFactories, context);

		return modelFactories;
	}

	private async Task<IEnumerable<Factory>> GetModelFactoriesWithAddresses(IReadOnlyCollection<DbModels.Factory> dbFactories, CompanyDbContext context)
	{
		var factoriesAddressIds = dbFactories.Select(x => x.AddressId).ToList();

		var factoriesAddressSelectQuery = @"SELECT * FROM address WHERE id IN ({0})";

		var parameters = string.Join(", ", Enumerable.Range(0, factoriesAddressIds.Count).Select(i => $"@p{i}"));

		factoriesAddressSelectQuery = string.Format(factoriesAddressSelectQuery, parameters);

		object[] sqlParameters = factoriesAddressIds
			.Select((id, index) => new NpgsqlParameter($"@p{index}", id))
			.ToArray();

		var dbFactoriesAddress = await context
			.Addresses
			.FromSqlRaw(factoriesAddressSelectQuery, sqlParameters)
			.ToListAsync();

		var modelFactories = dbFactories.Select(_mapper.Map).ToList();

		foreach (var factory in modelFactories)
		{
			var dbFactoryAddress = dbFactoriesAddress.FirstOrDefault(x => x.Id == factory.AddressId);
			if (dbFactoryAddress != null) factory.Address = _mapper.Map(dbFactoryAddress);
		}

		return modelFactories;
	}

	public async Task<int> AddAsync(Factory factory)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					INSERT INTO factory (phone, email, address_id)
					VALUES ({factory.Phone}, {factory.Email}, {factory.AddressId})");

		var factoryId = (await GetLastAddedEntity(TableName))!.Id;

		return factoryId;
	}

	public async Task DeleteAsync(int factoryId)
	{
		await DeleteEntityAsync(TableName, factoryId);
	}

	public async Task UpdateAsync(Factory factory)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					UPDATE factory
					SET
						phone = {factory.Phone},
						email = {factory.Email},
						address_id = {factory.AddressId}
					WHERE
						id = {factory.Id}");
	}
}