using Application.Customer.Commands;
using Application.Customer.Queries;
using Domain.Models;
using MediatR;

namespace UI.Stores;

public class CustomerStore
{
	private readonly IMediator _mediator;
	//private readonly List<Customer> _customers;
	//private Lazy<Task> _initializeLazy;

	//public IEnumerable<Customer> Customers => _customers;

	//public event Action<Customer>? CustomerAdded;
	//public event Action<int>? CustomerRemoved;
	//public event Action? DataChanges; 

	public CustomerStore(IMediator mediator)
	{
		_mediator = mediator;
		//_customers = new List<Customer>();
		//_initializeLazy = new Lazy<Task>(Initialize);
	}

	public async Task<Customer?> GetById(int customerId)
	{
		return await _mediator.Send(new GetCustomerByIdQuery { Id = customerId });
	}

	public async Task<IEnumerable<Customer>> GetAll()
	{
		try
		{
			return await _mediator.Send(new GetCustomersQuery());
			//await _initializeLazy.Value;
		}
		catch (Exception)
		{
			//_initializeLazy = new Lazy<Task>(Initialize);
			throw;
		}
	}

	public async Task Add(Customer customer, Address address)
	{
		var customerId = await _mediator.Send(new AddCustomerCommand { Customer = customer, Address = address });

		//if (customerId != null) customer.Id = (int)customerId;

		//_customers.Add(customer);

		//OnAddCustomer(customer);
	}

	private async Task Update(Customer customer)
	{
		await _mediator.Send(new UpdateCustomerCommand { Customer = customer });

		//_customers.RemoveAt(_customers.FindIndex(x => x.Id == customer.Id));
		//_customers.Add(customer);

		//OnAddCustomer(customer);
	}

	public async Task Delete(int customerId)
	{
		await _mediator.Send(new DeleteCustomerCommand { Id = customerId });

		//_customers.RemoveAt(_customers.FindIndex(x => x.Id == customerId));

		//CustomerRemoved?.Invoke(customerId);
		//DataChanges?.Invoke();
	}

	//public void OnAddCustomer(Customer customer)
	//{
	//	CustomerAdded?.Invoke(customer);
	//	DataChanges?.Invoke();
	//}

	//private async Task Initialize()
	//{
	//	try
	//	{
	//		var customers = await _mediator.Send(new GetCustomersQuery());

	//		_customers.Clear();
	//		_customers.AddRange(customers);
	//	}
	//	catch (Exception)
	//	{
	//		throw;
	//	}
	//}
}