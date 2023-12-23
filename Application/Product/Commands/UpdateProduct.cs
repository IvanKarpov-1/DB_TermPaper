using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Product.Commands;

public class UpdateProductCommand : IRequest<Unit>
{
	public Domain.Models.Product? Product { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
	private readonly IProductRepository _productRepository;

	public UpdateProductCommandHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
	{
		if (request.Product != null) await _productRepository.UpdateAsync(request.Product);
		return Unit.Value;
	}
}