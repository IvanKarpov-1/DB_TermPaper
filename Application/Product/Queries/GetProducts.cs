using Application.Abstractions.Repositories;
using Application.Common;
using MediatR;

namespace Application.Product.Queries;

public class GetProductsQuery : IRequest<IEnumerable<Domain.Models.Product>>
{
	public PagingParams? PagingParams { get; set; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Domain.Models.Product>>
{
	private readonly IProductRepository _productRepository;

	public GetProductsQueryHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<IEnumerable<Domain.Models.Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
	{
		var products = await _productRepository.GetAllAsync(request.PagingParams);
		return products;
	}
}