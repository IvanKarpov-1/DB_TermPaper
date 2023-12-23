namespace Application.Abstractions.Repositories;

public interface IAnalyticsRepository
{
	public Task<decimal> GetTotalEstimatedOutput();
	public Task<int> GetNumberOfProductsPerYear();
	public Task<int> GetNumberOfOrders();
}