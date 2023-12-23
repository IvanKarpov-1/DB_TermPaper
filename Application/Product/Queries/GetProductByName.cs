using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Product.Queries;

public class GetProductByNameQuery : IRequest<IEnumerable<Domain.Models.Product>>
{
	public string Name { get; set; } = " ";
}

public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IEnumerable<Domain.Models.Product>>
{
	private readonly IProductRepository _productRepository;

	public GetProductByNameQueryHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<IEnumerable<Domain.Models.Product>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
	{
		return await _productRepository.FindByNameAsync(request.Name);
	}
}