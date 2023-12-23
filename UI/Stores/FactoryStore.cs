using Application.Factory.Commands;
using Application.Factory.Queries;
using Domain.Models;
using MediatR;

namespace UI.Stores;

public class FactoryStore
{
	private readonly IMediator _mediator;
	//private readonly List<Factory> _factories;
	//private Lazy<Task> _initializeLazy;

	//public IEnumerable<Factory> Factories => _factories;

	//public event Action<Factory>? FactoryAdded;
	//public event Action<int>? FactoryRemoved;

	public FactoryStore(IMediator mediator)
	{
		_mediator = mediator;
		//_factories = new List<Factory>();
		//_initializeLazy = new Lazy<Task>(Initialize);
	}

	public async Task<Factory?> GetById(int factoryId)
	{
		return await _mediator.Send(new GetFactoryByIdQuery { Id = factoryId });
	}

	public async Task<IEnumerable<Factory>> GetAll()
	{
		try
		{
			var factories = await _mediator.Send(new GetFactoriesQuery());
			return factories;
		}
		catch (Exception)
		{
			//_initializeLazy = new Lazy<Task>(Initialize);
			throw;
		}
	}

	public async Task Add(Factory factory, Address address)
	{
		await _mediator.Send(new AddFactoryCommand { Factory = factory, Address = address });

		//_factories.Add(factory);

		//OnAddFactory(factory);
	}

	public async Task Update(Factory factory)
	{
		await _mediator.Send(new UpdateFactoryCommand { Factory = factory });

		//_factories.RemoveAt(_factories.FindIndex(x => x.Id == factory.Id));
		//_factories.Add(factory);

		//OnAddFactory(factory);
	}

	public async Task Delete(int factoryId)
	{
		await _mediator.Send(new DeleteFactoryCommand { Id = factoryId });

		//_factories.RemoveAt(_factories.FindIndex(x => x.Id == factoryId));

		//FactoryRemoved?.Invoke(factoryId);
	}

	//private void OnAddFactory(Factory factory) => FactoryAdded?.Invoke(factory);

	//private async Task Initialize()
	//{
	//	try
	//	{
	//		var factories = await _mediator.Send(new GetFactoriesQuery());

	//		_factories.Clear();
	//		_factories.AddRange(factories);
	//	}
	//	catch (Exception)
	//	{
	//		throw;
	//	}
	//}
}