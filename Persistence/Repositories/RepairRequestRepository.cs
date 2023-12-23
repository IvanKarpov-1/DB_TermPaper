using Application.Abstractions.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class RepairRequestRepository : GenericRepository<DbModels.RepairRequest>, IRepairRequestRepository
{
	private readonly ICompanyDbContextFactory _companyDbContextFactory;
	private readonly MapperlyMapper _mapper;
	private const string TableName = "repair_request";

	public RepairRequestRepository(ICompanyDbContextFactory companyDbContextFactory, MapperlyMapper mapper)
		: base(companyDbContextFactory)
	{
		_companyDbContextFactory = companyDbContextFactory;
		_mapper = mapper;
	}

	public async Task<RepairRequest?> FindByIdAsync(int repairRequestId)
	{
		var dbRepairRequest = await FindEntityByColumnAsync(TableName, "id", repairRequestId);

		if (dbRepairRequest == null) return null;

		var domainRepairRequest = _mapper.Map(dbRepairRequest);

		var statusName = RepairRequestStatus.FromValue(domainRepairRequest.StatusId).DisplayName;

		domainRepairRequest.StatusName = statusName;

		return domainRepairRequest;
	}

	public async Task<RepairRequest?> GetLastAdded()
	{
		var dbAddress = await GetLastAddedEntity(TableName);

		return dbAddress == null ? null : _mapper.Map(dbAddress);
	}

	public async Task<IEnumerable<RepairRequest>> GetAllCustomerRepairRequestsAsync(int customerId)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var dbRepairRequests = await context
			.RepairRequests
			.FromSql($@"
					SELECT
						*
					FROM
						repair_request
					WHERE
						customer_id = {customerId}").ToListAsync();

		var domainRepairRequests = dbRepairRequests.Select(_mapper.Map).ToList();

		foreach (var domainRepairRequest in domainRepairRequests)
		{
			domainRepairRequest.StatusName = RepairRequestStatus.FromValue(domainRepairRequest.StatusId).DisplayName;
		}

		return domainRepairRequests;
	}

	public async Task<int> AddAsync(RepairRequest repairRequest)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					INSERT INTO repair_request (request_date, status_id, description, customer_id)
					VALUES ({repairRequest.RequestDate}, {repairRequest.StatusId}, {repairRequest.Description}, {repairRequest.CustomerId})");

		var repairRequestId = (await GetLastAdded())!.Id;

		return repairRequestId;
	}

	public async Task DeleteAsync(int repairRequestId)
	{
		await DeleteEntityAsync(TableName, repairRequestId);
	}

	public async Task UpdateAsync(RepairRequest repairRequest)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var statusId = RepairRequestStatus.FromDisplayName(repairRequest.StatusName).Id;

		await context
			.Database
			.ExecuteSqlAsync($@"
					UPDATE repair_request
					SET
						request_date = {repairRequest.RequestDate},
						status_id = {statusId},
						description = {repairRequest.Description},
						customer_id = {repairRequest.CustomerId}
					WHERE
						id = {repairRequest.Id}");
	}
}