using Application.Abstractions.Repositories;
using Application.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class CustomerRepository : GenericRepository<DbModels.Customer>, ICustomerRepository
{
	private readonly ICompanyDbContextFactory _companyDbContextFactory;
	private readonly MapperlyMapper _mapper;
	private const string TableName = "customer";

	public CustomerRepository(ICompanyDbContextFactory companyDbContextFactory, MapperlyMapper mapper)
		: base(companyDbContextFactory)
	{
		_companyDbContextFactory = companyDbContextFactory;
		_mapper = mapper;
	}

	public async Task<Customer?> FindByIdAsync(int customerId)
	{
		var dbCustomer = await FindEntityByColumnAsync(TableName, "id", customerId);

		if (dbCustomer == null) return null;

		var domainCustomer = _mapper.Map(dbCustomer);

		return domainCustomer;
	}

	public async Task<Customer?> FindByNameAsync(string name)
	{
		var dbCustomer = await FindEntityByColumnAsync(TableName, "name", name);

		if (dbCustomer == null) return null;

		var domainCustomer = _mapper.Map(dbCustomer);

		return domainCustomer;
	}

	public async Task<Customer?> GetLastAdded()
	{
		var dbCustomer = await GetLastAddedEntity(TableName);

		return dbCustomer == null ? null : _mapper.Map(dbCustomer);
	}

	public async Task<IEnumerable<Customer>> GetAllAsync(PagingParams? pagingParams = null)
	{
		pagingParams ??= PagingParams.Default;

		var dbCustomers = await GetAllEntitiesAsync(TableName, pagingParams);

		var modelCustomers = dbCustomers.Select(_mapper.Map);

		return modelCustomers;
	}

	public async Task<int> AddAsync(Customer customer)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					INSERT INTO customer (name, phone, email)
					VALUES ({customer.Name}, {customer.Phone}, {customer.Email})");

		var customerId = (await GetLastAdded())!.Id;

		return customerId;
	}

	public async Task DeleteAsync(int customerId)
	{
		await DeleteEntityAsync(TableName, customerId);
	}

	public async Task UpdateAsync(Customer customer)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					UPDATE customer
					SET 
					    name = {customer.Name},
						phone = {customer.Phone},
						email = {customer.Email}
					WHERE
						id = {customer.Id}");
	}
}