using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Product.Commands;

public class DeleteProductCommand : IRequest<Unit>
{
	public int Id { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
	private readonly IProductRepository _productRepository;

	public DeleteProductCommandHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
	{
		await _productRepository.DeleteAsync(request.Id);
		return Unit.Value;
	}
}