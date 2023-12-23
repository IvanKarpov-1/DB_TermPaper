using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.RepairRequest;

namespace UI.Commands.RepairRequest;

public class DeleteRepairRequestCommand : AsyncCommandBase
{
	private readonly RepairRequestDetailsViewModel _repairRequestDetailsViewModel;
	private readonly RepairRequestStore _repairRequestStore;
	private readonly NavigationStore _navigationStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public DeleteRepairRequestCommand(RepairRequestDetailsViewModel repairRequestDetailsViewModel, RepairRequestStore repairRequestStore, NavigationStore navigationStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_repairRequestDetailsViewModel = repairRequestDetailsViewModel;
		_repairRequestStore = repairRequestStore;
		_navigationStore = navigationStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _repairRequestStore.Delete(_repairRequestDetailsViewModel.RepairRequest.Id);
			_repairRequestDetailsViewModel.NavigateBackCommand.Execute(null);
			_navigationStore.ClearForwardHistory();
			_snackbarMessageQueue.Enqueue("Запит було успішно видалено");
		}
		catch (Exception)
		{
			throw;
		}
	}
}