using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Customer.Commands;

public class DeleteCustomerCommand : IRequest<Unit>
{
	public int Id { get; set; }
}

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
{
	private readonly ICustomerRepository _customerRepository;

	public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
	{
		_customerRepository = customerRepository;
	}

	public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
	{
		await _customerRepository.DeleteAsync(request.Id);
		return Unit.Value;
	}
}