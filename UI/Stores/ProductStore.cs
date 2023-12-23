using System.Windows.Media.Animation;
using Application.Product.Commands;
using Application.Product.Queries;
using Domain.Models;
using MediatR;

namespace UI.Stores;

public class ProductStore
{
	private readonly IMediator _mediator;
	//private readonly HashSet<Product> _products;
	//private readonly Dictionary<int, List<Product>> _factoryProducts;
	//private Lazy<Task> _initializeLazy;

	//public IEnumerable<Product> Products => _products.ToList();

	//public IEnumerable<Product> ProductsFromFactory(int factoryId)
	//{
	//	if (_factoryProducts.ContainsKey(factoryId) == false)
	//		_factoryProducts[factoryId] = new List<Product>();
	//	return _factoryProducts[factoryId];
	//}

	//public event Action<Product>? ProductAdded;
	//public event Action<int>? ProductRemoved;
	//public event Action? DataChanges;

	public ProductStore(IMediator mediator)
	{
		_mediator = mediator;
		//_products = new HashSet<Product>();
		//_factoryProducts = new Dictionary<int, List<Product>>();
		//_initializeLazy = new Lazy<Task>(Initialize);
	}

	public async Task<Product?> GetById(int productId)
	{
		return await _mediator.Send(new GetProductByIdQuery { Id = productId });
	}

	public async Task<IEnumerable<Product>> GetByName(string name)
	{
		var products = await _mediator.Send(new GetProductByNameQuery { Name = name });
		return products;
	}

	public async Task<IEnumerable<Product>> GetAll()
	{
		try
		{
			return await _mediator.Send(new GetProductsQuery());
		}
		catch (Exception)
		{
			throw;
		}
	}

	public async Task<IEnumerable<Product>> GetProductsFromSpecificFactory(int factoryId)
	{
		//if (_factoryProducts.ContainsKey(factoryId)) return;

		var products = (await _mediator.Send(new GetProductsFromFactoryQuery { FactoryId = factoryId })).ToList();
		return products;

		//if (_factoryProducts.TryGetValue(factoryId, out var value))
		//	value.Clear();
		//else
		//	_factoryProducts[factoryId] = new List<Product>();

		//_factoryProducts[factoryId].AddRange(products);

		//foreach (var product in products)
		//{
		//	_products.Add(product);
		//}
	}

	public async Task<IEnumerable<Product>> GetProductsFromSpecificOrder(int orderId)
	{
		return await _mediator.Send(new GetProductsFromOrderQuery { OrderId = orderId });
	}

	public async Task Add(Product product, int factoryId)
	{
		/*var productId =*/ await _mediator.Send(new AddProductCommand { Product = product, FactoryId = factoryId });

		//if (productId != null) product.Id = (int)productId;

		//_products.Add(product);

		//if (_factoryProducts.ContainsKey(factoryId) == false)
		//	_factoryProducts[factoryId] = new List<Product>();
		//_factoryProducts[factoryId].Add(product);

		//OnAddProduct(product);
	}

	public async Task Update(Product product)
	{
		await _mediator.Send(new UpdateProductCommand { Product = product });

		//_products.RemoveWhere(x => x.Id == product.Id);
		//_products.Add(product);

		//var factoryId = _factoryProducts.FirstOrDefault(x => x.Value.Exists(x => x.Id == product.Id)).Key;

		//if (_factoryProducts.ContainsKey(factoryId) == false)
		//{
		//	_factoryProducts[factoryId] = new List<Product> { product };
		//}
		//else
		//{
		//	_factoryProducts[factoryId].RemoveAt(_factoryProducts[factoryId].FindIndex(x => x.Id == product.Id));
		//	_factoryProducts[factoryId].Add(product);
		//}

		//OnAddProduct(product);
	}

	public async Task Delete(int productId)
	{
		await _mediator.Send(new DeleteProductCommand { Id = productId });

		//_products.RemoveWhere(x => x.Id == productId);

		//var factoryId = _factoryProducts.FirstOrDefault(x => x.Value.Exists(x => x.Id == productId)).Key;

		//if (_factoryProducts.ContainsKey(factoryId))
		//	_factoryProducts[factoryId].RemoveAt(_factoryProducts[factoryId].FindIndex(x => x.Id == productId));

		//ProductRemoved?.Invoke(productId);
		//DataChanges?.Invoke();
	}

	//private void OnAddProduct(Product product)
	//{
	//	ProductAdded?.Invoke(product);
	//	DataChanges?.Invoke();
	//}

	//private async Task Initialize()
	//{
	//	var products = await _mediator.Send(new GetProductsQuery());

	//	await Task.Delay(3000);

	//	foreach (var product in products)
	//	{
	//		_products.Add(product);
	//	}
	//}
}