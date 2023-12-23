using System.Collections.ObjectModel;
using System.Windows.Input;
using Domain.Models;
using MaterialDesignThemes.Wpf;
using UI.Commands;
using UI.Commands.RepairRequest;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.ViewModels.RepairRequest;

public class AddRepairRequestViewModel : ViewModelBase
{
	public AddRepairRequestViewModel(
		int customerId,
		NavigationStore navigationStore,
		RepairRequestStore repairRequestStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_repairRequest = new RepairRequestListItemViewModel(new Domain.Models.RepairRequest { CustomerId = customerId });
		_repairRequestStatuses = new ObservableCollection<RepairRequestStatus>(RepairRequestStatus.GetAll());

		SaveCommand = new AddRepairRequestCommand(this, repairRequestStore, navigationStore, snackbarMessageQueue);
		CancelCommand = new NavigateBackCommand(navigationStore);
	}

	public static AddRepairRequestViewModel LoadViewModel(
		int customerId,
		NavigationStore navigationStore,
		RepairRequestStore repairRequestStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		var viewModel = new AddRepairRequestViewModel(
			customerId,
			navigationStore,
			repairRequestStore,
			snackbarMessageQueue);

		return viewModel;
	}

	private RepairRequestListItemViewModel _repairRequest;
	public RepairRequestListItemViewModel RepairRequest
	{
		get => _repairRequest;
		set => SetField(ref _repairRequest, value);
	}

	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }

	private readonly ObservableCollection<RepairRequestStatus> _repairRequestStatuses;
	public IEnumerable<RepairRequestStatus> RepairRequestStatuses => _repairRequestStatuses;
}