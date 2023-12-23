using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Product.Queries;

public class GetProductByIdQuery : IRequest<Domain.Models.Product?>
{
	public int Id { get; set; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Domain.Models.Product?>
{
	private readonly IProductRepository _productRepository;

	public GetProductByIdQueryHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<Domain.Models.Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
	{
		return await _productRepository.FindByIdAsync(request.Id);
	}
}