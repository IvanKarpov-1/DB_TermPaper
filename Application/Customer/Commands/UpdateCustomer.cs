using Application.Abstractions.Repositories;
using MediatR;

namespace Application.Customer.Commands;

public class UpdateCustomerCommand : IRequest<Unit>
{
	public Domain.Models.Customer? Customer { get; set; }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
	private readonly ICustomerRepository _customerRepository;

	public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
	{
		_customerRepository = customerRepository;
	}

	public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
	{
		if (request.Customer != null) await _customerRepository.UpdateAsync(request.Customer);
		return Unit.Value;
	}
}