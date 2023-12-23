using Application.Abstractions.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class AddressRepository : GenericRepository<DbModels.Address>, IAddressRepository
{
	private readonly ICompanyDbContextFactory _companyDbContextFactory;
	private readonly MapperlyMapper _mapper;
	private const string TableName = "address";

	public AddressRepository(ICompanyDbContextFactory companyDbContextFactory, MapperlyMapper mapper)
		:base(companyDbContextFactory)
	{
		_companyDbContextFactory = companyDbContextFactory;
		_mapper = mapper;
	}

	public async Task<Address?> FindByIdAsync(int addressId)
	{
		var dbAddress = await FindEntityByColumnAsync(TableName, "id", addressId);

		if (dbAddress == null) return null;

		var domainAddress = _mapper.Map(dbAddress);

		return domainAddress;
	}

	public async Task<Address?> GetLastAdded()
	{
		var dbAddress = await GetLastAddedEntity(TableName);

		return dbAddress == null ? null : _mapper.Map(dbAddress);
	}

	public async Task<IEnumerable<Address>> GetAllCustomerAddressesAsync(int customerId)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var dbAddresses = await context
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
					FROM
						customer_address as ca
						JOIN address     as a  ON ca.address_id = a.id
					WHERE
						ca.customer_id = {customerId}").ToListAsync();

		var domainAddresses = dbAddresses.Select(_mapper.Map);

		return domainAddresses;
	}

	public async Task<int> AddAsync(Address address)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					INSERT INTO address (country, region, city, address_line1, address_line2, post_code)
					VALUES ({address.Country}, {address.Region}, {address.City}, {address.AddressLine1}, 
							{address.AddressLine2}, {address.PostCode})");

		var addressId = (await GetLastAdded())!.Id;

		return addressId;
	}

	public async Task DeleteAsync(int addressId)
	{
		await DeleteEntityAsync(TableName, addressId);
	}

	public async Task UpdateAsync(Address address)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					UPDATE address
					SET
						country = {address.Country},
						region = {address.Region},
						city = {address.City},
						address_line1 = {address.AddressLine1},
						address_line2 = {address.AddressLine2},
						post_code = {address.PostCode}
					WHERE
						id = {address.Id}");
	}
}