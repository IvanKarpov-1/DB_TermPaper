using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Persistence.DbContext;

public class CompanyDbContextFactory : ICompanyDbContextFactory
{
	private readonly string _connectionString;

	public CompanyDbContextFactory(string connectionString)
	{
		_connectionString = connectionString;
	}

	public CompanyDbContext CreateCompanyDbContext()
	{
		var options = new DbContextOptionsBuilder<CompanyDbContext>()
			.UseNpgsql(_connectionString)
			.EnableSensitiveDataLogging()
			.LogTo(message => Debug.WriteLine(message))
			.Options;
		return new CompanyDbContext(options);
	}
}