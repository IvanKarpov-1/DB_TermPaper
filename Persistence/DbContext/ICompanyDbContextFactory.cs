namespace Persistence.DbContext;

public interface ICompanyDbContextFactory
{
	CompanyDbContext CreateCompanyDbContext();
}