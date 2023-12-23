using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.RepairRequest;

namespace UI.Commands.RepairRequest;

public class UpdateRepairRequestCommand : AsyncCommandBase
{
	private readonly RepairRequestDetailsViewModel _repairRequestDetailsViewModel;
	private readonly RepairRequestStore _repairRequestStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public UpdateRepairRequestCommand(RepairRequestDetailsViewModel repairRequestDetailsViewModel, RepairRequestStore repairRequestStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_repairRequestDetailsViewModel = repairRequestDetailsViewModel;
		_repairRequestStore = repairRequestStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			_repairRequestDetailsViewModel.IsEditing = false;
			await _repairRequestStore.Update(_repairRequestDetailsViewModel.RepairRequest.RepairRequest);
			_snackbarMessageQueue.Enqueue("Інформація про запит успісшно змінена");
		}
		catch (Exception e)
		{
			throw;
		}
	}
}