using MaterialDesignThemes.Wpf;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.RepairRequest;

namespace UI.Commands.RepairRequest;

public class AddRepairRequestCommand : AsyncCommandBase
{
	private readonly AddRepairRequestViewModel _addRepairRequestViewModel;
	private readonly RepairRequestStore _repairRequestStore;
	private readonly NavigationStore _navigationStore;
	private readonly ISnackbarMessageQueue _snackbarMessageQueue;

	public AddRepairRequestCommand(AddRepairRequestViewModel addRepairRequestViewModel, RepairRequestStore repairRequestStore, NavigationStore navigationStore, ISnackbarMessageQueue snackbarMessageQueue)
	{
		_addRepairRequestViewModel = addRepairRequestViewModel;
		_repairRequestStore = repairRequestStore;
		_navigationStore = navigationStore;
		_snackbarMessageQueue = snackbarMessageQueue;
	}

	public override async Task ExecuteAsync(object? parameter)
	{
		try
		{
			await _repairRequestStore.Add(_addRepairRequestViewModel.RepairRequest.RepairRequest);
			_addRepairRequestViewModel.CancelCommand.Execute(null);
			_navigationStore.ClearForwardHistory();
			_snackbarMessageQueue.Enqueue("Запит успішно додано");
		}
		catch (Exception)
		{
			throw;
		}
	}
}