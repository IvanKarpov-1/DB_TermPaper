using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Customer;

namespace UI.Commands.RepairRequest;

public class LoadRepairRequestsCommand : AsyncCommandBase
{
	private readonly CustomerDetailsViewModel _customerDetailsViewModel;
	private readonly RepairRequestStore _repairRequestStore;
	private readonly int _customerId;

	public LoadRepairRequestsCommand(CustomerDetailsViewModel customerDetailsViewModel, RepairRequestStore repairRequestStore, int customerId)
	{
		_customerDetailsViewModel = customerDetailsViewModel;
		_repairRequestStore = repairRequestStore;
		_customerId = customerId;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			var repairRequests = await _repairRequestStore.GetRepairRequestsOfSpecificCustomer(_customerId);
			_customerDetailsViewModel.UpdateRepairRequests(repairRequests);
		}
		catch (Exception)
		{
			throw;
		}
	}
}