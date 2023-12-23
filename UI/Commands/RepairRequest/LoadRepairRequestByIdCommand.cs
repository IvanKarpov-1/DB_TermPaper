using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.RepairRequest;

namespace UI.Commands.RepairRequest;

public class LoadRepairRequestByIdCommand : AsyncCommandBase
{
	private readonly RepairRequestDetailsViewModel _repairRequestDetailsViewModel;
	private readonly RepairRequestStore _repairRequestStore;

	public LoadRepairRequestByIdCommand(RepairRequestDetailsViewModel repairRequestDetailsViewModel, RepairRequestStore repairRequestStore)
	{
		_repairRequestDetailsViewModel = repairRequestDetailsViewModel;
		_repairRequestStore = repairRequestStore;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			int.TryParse(parameter?.ToString(), out var id);
			var repairRequest = await _repairRequestStore.GetById(id);
			if (repairRequest != null) _repairRequestDetailsViewModel.UpdateRepairRequest(repairRequest);
		}
		catch (Exception)
		{
			throw;
		}
	}
}