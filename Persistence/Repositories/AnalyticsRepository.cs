using Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
	private readonly ICompanyDbContextFactory _companyDbContextFactory;

	public AnalyticsRepository(ICompanyDbContextFactory companyDbContextFactory)
	{
		_companyDbContextFactory = companyDbContextFactory;
	}

	public async Task<decimal> GetTotalEstimatedOutput()
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();
			
		var totalEstimatedOutput = context
			.Database
			.SqlQuery<decimal>($@"
					SELECT
					    SUM(selling_price * number_per_year) AS ""Value""
					FROM
					    product").FirstOrDefault();
			
		return totalEstimatedOutput;
	}

	public async Task<int> GetNumberOfProductsPerYear()
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var numberOfProductsPerYear = await context
			.Database
			.SqlQuery<int>($@"
					SELECT
					    SUM(number_per_year) AS ""Value""
					FROM
					    product").FirstOrDefaultAsync();

		return numberOfProductsPerYear;
	}

	public async Task<int> GetNumberOfOrders()
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var numberOfOrders = await context
			.Database
			.SqlQuery<int>($@"
					SELECT
					    COUNT(*) AS ""Value""
					FROM
					    ""order""").FirstOrDefaultAsync();

		return numberOfOrders;
	}
}