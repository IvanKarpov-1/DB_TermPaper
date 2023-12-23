using Application.Abstractions.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class CustomerAddressRepository : GenericRepository<DbModels.CustomerAddress>, ICustomerAddressRepository
{
	private readonly ICompanyDbContextFactory _companyDbContextFactory;
	private const string TableName = "customer_address";

	public CustomerAddressRepository(ICompanyDbContextFactory companyDbContextFactory) : base(companyDbContextFactory)
	{
		_companyDbContextFactory = companyDbContextFactory;
	}

	public async Task AddAsync(CustomerAddress customerAddress)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					INSERT INTO customer_address (customer_id, address_id)
					VALUES ({customerAddress.CustomerId}, {customerAddress.AddressId})");
	}

	public async Task DeleteAsync(int customerAddressId)
	{
		await DeleteEntityAsync(TableName, customerAddressId);
	}
}