using Application.Address.Commands;
using Application.Address.Queries;
using Domain.Models;
using MediatR;

namespace UI.Stores;

public class AddressStore
{
	private readonly IMediator _mediator;
	//private readonly HashSet<Address> _addresses;
	//private readonly Dictionary<int, List<Address>> _customerAddress;

	//public IEnumerable<Address> Addresses => _addresses;

	//public IEnumerable<Address> AddressesFromCustomer(int customerId)
	//{
	//	if (_customerAddress.ContainsKey(customerId) == false)
	//		_customerAddress[customerId] = new List<Address>();
	//	return _customerAddress[customerId];
	//}

	//public event Action<Address>? AddressAdded;
	//public event Action<int>? AddressRemoved;

	public AddressStore(IMediator mediator)
	{
		_mediator = mediator;
		//_addresses = new HashSet<Address>();
		//_customerAddress = new Dictionary<int, List<Address>>();
	}

	public async Task<Address?> GetById(int addressId)
	{
		return await _mediator.Send(new GetAddressByIdQuery { Id = addressId });
	}

	public async Task<IEnumerable<Address>> GetAddressesOfSpecificCustomer(int customerId)
	{
		//if (_customerAddress.ContainsKey(customerId)) return;
		var addresses = (await _mediator.Send(new GetAllCustomerAddressesQuery { CustomerId = customerId })).ToList();
		return addresses;

		//if (_customerAddress.TryGetValue(customerId, out var value))
		//	value.Clear();
		//else
		//	_customerAddress[customerId] = new List<Address>();

		//_customerAddress[customerId].AddRange(addresses);

		//foreach (var address in addresses)
		//{
		//	_addresses.Add(address);
		//}
	}

	public async Task Add(Address address, int customerId)
	{
		var addressId = await _mediator.Send(new AddCustomerAddressCommand { Address = address, CustomerId = customerId });

		//if (addressId != null) address.Id = (int)addressId;

		//_addresses.Add(address);

		//if (_customerAddress.ContainsKey(customerId) == false)
		//	_customerAddress[customerId] = new List<Address>();
		//_customerAddress[customerId].Add(address);

		//OnAddAddress(address);
	}

	public async Task Update(Address address)
	{
		await _mediator.Send(new UpdateAddressCommand { Address = address });

		//_addresses.RemoveWhere(x => x.Id == address.Id);
		//_addresses.Add(address);

		//var customerId = _customerAddress.FirstOrDefault(x => x.Value.Exists(x => x.Id == address.Id)).Key;

		//if (_customerAddress.ContainsKey(customerId) == false)
		//{
		//	_customerAddress[customerId] = new List<Address> { address };
		//}
		//else
		//{
		//	_customerAddress[customerId].RemoveAt(_customerAddress[customerId].FindIndex(x => x.Id == address.Id));
		//	_customerAddress[customerId].Add(address);
		//}

		//OnAddAddress(address);
	}

	public async Task Delete(int addressId)
	{
		await _mediator.Send(new DeleteAddressCommand { Id = addressId });

		//_addresses.RemoveWhere(x => x.Id == addressId);

		//var customerId = _customerAddress.FirstOrDefault(x => x.Value.Exists(x => x.Id == addressId)).Key;

		//if (_customerAddress.ContainsKey(customerId))
		//	_customerAddress[customerId].RemoveAt(_customerAddress[customerId].FindIndex(x => x.Id == addressId));

		//AddressRemoved?.Invoke(addressId);
	}

	//private void OnAddAddress(Address address) => AddressAdded?.Invoke(address);
}