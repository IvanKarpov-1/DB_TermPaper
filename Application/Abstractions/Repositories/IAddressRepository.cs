namespace Application.Abstractions.Repositories;

public interface IAddressRepository
{
	public Task<Domain.Models.Address?> FindByIdAsync(int addressId);
	public Task<Domain.Models.Address?> GetLastAdded();
	public Task<IEnumerable<Domain.Models.Address>> GetAllCustomerAddressesAsync(int customerId);
	public Task<int> AddAsync(Domain.Models.Address address);
	public Task DeleteAsync(int addressId);
	public Task UpdateAsync(Domain.Models.Address address);
}