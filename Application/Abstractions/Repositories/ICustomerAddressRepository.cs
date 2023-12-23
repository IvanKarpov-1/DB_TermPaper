namespace Application.Abstractions.Repositories;

public interface ICustomerAddressRepository
{
	public Task AddAsync(Domain.Models.CustomerAddress customerAddress);
	public Task DeleteAsync(int customerAddressId);
}