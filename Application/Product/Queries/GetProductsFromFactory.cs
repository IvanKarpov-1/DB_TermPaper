using Application.Abstractions.Repositories;
using Application.Common;
using MediatR;

namespace Application.Product.Queries;

public class GetProductsFromFactoryQuery : IRequest<IEnumerable<Domain.Models.Product>>
{
	public PagingParams? PagingParams { get; set; }
	public int FactoryId { get; set; }
}

public class GetProductsFromFactoryQueryHandler : IRequestHandler<GetProductsFromFactoryQuery, IEnumerable<Domain.Models.Product>>
{
	private readonly IProductRepository _productRepository;

	public GetProductsFromFactoryQueryHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<IEnumerable<Domain.Models.Product>> Handle(GetProductsFromFactoryQuery request, CancellationToken cancellationToken)
	{
		var products = await _productRepository.GetAllFromSpecificFactoryAsync(request.FactoryId, request.PagingParams);
		return products;
	}
}