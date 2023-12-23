using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Product.Queries;

public class GetProductsFromOrderQuery : IRequest<IEnumerable<Domain.Models.Product>>
{
	public int OrderId { get; set; }
}

public class GetProductsFromOrderQueryHandler : IRequestHandler<GetProductsFromOrderQuery, IEnumerable<Domain.Models.Product>>
{
	private readonly IProductRepository _productRepository;


	public GetProductsFromOrderQueryHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<IEnumerable<Domain.Models.Product>> Handle(GetProductsFromOrderQuery request, CancellationToken cancellationToken)
	{
		var products = await _productRepository.GetAllFromSpecificOrderAsync(request.OrderId);
		return products;
	}
}
