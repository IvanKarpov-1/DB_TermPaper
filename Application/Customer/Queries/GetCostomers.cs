using Application.Abstractions.Repositories;
using Application.Common;
using MediatR;

namespace Application.Customer.Queries;

public class GetCustomersQuery : IRequest<IEnumerable<Domain.Models.Customer>>
{
	public PagingParams? PagingParams { get; set; }
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<Domain.Models.Customer>>
{
	private readonly ICustomerRepository _customerRepository;

	public GetCustomersQueryHandler(ICustomerRepository customerRepository)
	{
		_customerRepository = customerRepository;
	}

	public async Task<IEnumerable<Domain.Models.Customer>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
	{
		var products = await _customerRepository.GetAllAsync(request.PagingParams);
		return products;
	}
}