using Application.Abstractions.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DbContext;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
		IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("CompanyDB");

		if (connectionString != null)
			services.AddSingleton<ICompanyDbContextFactory>(new CompanyDbContextFactory(connectionString));

		services.AddTransient<MapperlyMapper>();

		services.AddTransient<IAddressRepository, AddressRepository>();
		services.AddTransient<IAnalyticsRepository, AnalyticsRepository>();
		services.AddTransient<ICustomerAddressRepository, CustomerAddressRepository>();
		services.AddTransient<ICustomerRepository, CustomerRepository>();
		services.AddTransient<IFactoryRepository, FactoryRepository>();
		services.AddTransient<IOrderRepository, OrderRepository>();
		services.AddTransient<IProductRepository, ProductRepository>();
		services.AddTransient<IRepairRequestRepository, RepairRequestRepository>();

		return services;
	}
}