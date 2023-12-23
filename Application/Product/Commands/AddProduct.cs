using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Product.Commands;

public class AddProductCommand : IRequest<int?>
{
	public Domain.Models.Product? Product { get; set; }
	public int FactoryId { get; set; }
}

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int?>
{
	private readonly IProductRepository _productRepository;

	public AddProductCommandHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<int?> Handle(AddProductCommand request, CancellationToken cancellationToken)
	{
		if (request.Product == null) return null;
		var productId = await _productRepository.AddAsync(request.Product, request.FactoryId);
		return productId;
	}
}