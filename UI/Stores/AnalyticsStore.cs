using Application.Analytics.Queries;
using MediatR;

namespace UI.Stores;

public class AnalyticsStore
{
	private readonly IMediator _mediator;
	private decimal _totalEstimatedOutput;
	private int _numberOfProductsPerYear;
	private int _numberOfOrders;

	public decimal TotalEstimatedOutput => _totalEstimatedOutput;
	public int NumberOfProductsPerYear => _numberOfProductsPerYear;
	public int NumberOfOrders => _numberOfOrders;

	public AnalyticsStore(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task Load()
	{
		try
		{
			_totalEstimatedOutput = await _mediator.Send(new GetTotalEstimatedOutputQuery());
			_numberOfProductsPerYear = await _mediator.Send(new GetNumberOfProductsPerYearQuery());
			_numberOfOrders = await _mediator.Send(new GetNumberOfOrdersQuery());
		}
		catch (Exception)
		{
			throw;
		}
	}
}