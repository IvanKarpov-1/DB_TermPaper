using Domain.Models;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Base;
using UI.Commands.RepairRequest;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.ViewModels.RepairRequest;

public class RepairRequestDetailsViewModel : ViewModelBase, ILoadingViewModel, IEditableViewModel
{
	private readonly int _repairRequestId;

	public RepairRequestDetailsViewModel(
		int repairRequestId,
		NavigationStore navigationStore,
		RepairRequestStore repairRequestStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_repairRequestId = repairRequestId;
		_repairRequestStatuses = new ObservableCollection<RepairRequestStatus>(RepairRequestStatus.GetAll());

		LoadRepairRequestById = new LoadRepairRequestByIdCommand(this, repairRequestStore);
		NavigateBackCommand = new NavigateBackCommand(navigationStore);
		EditCommand = new RelayCommand(OnEditExecuted, _ => true);
		SaveCommand = new UpdateRepairRequestCommand(this, repairRequestStore, snackbarMessageQueue);
		CancelCommand = new RelayCommand(OnCancelExecuted, _ => true);
		DeleteCommand = new DeleteRepairRequestCommand(this, repairRequestStore, navigationStore, snackbarMessageQueue);
	}

	public static RepairRequestDetailsViewModel LoadViewModel(
		int repairRequestId,
		NavigationStore navigationStore,
		RepairRequestStore repairRequestStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		var viewModel = new RepairRequestDetailsViewModel(
			repairRequestId,
			navigationStore,
			repairRequestStore,
			snackbarMessageQueue);
		viewModel.Load();
		return viewModel;
	}

	public async void Load()
	{
		await LoadRepairRequestById.ExecuteAsync(_repairRequestId);
	}

	private RepairRequestListItemViewModel _repairRequest = null!;
	private RepairRequestListItemViewModel _tempRepairRequest = null!;
	public RepairRequestListItemViewModel RepairRequest
	{
		get => _repairRequest;
		set => SetField(ref _repairRequest, value);
	}

	private bool _isLoading = true;
	public bool IsLoading
	{
		get => _isLoading;
		set => SetField(ref _isLoading, value);
	}

	private bool _isEditing;
	public bool IsEditing
	{
		get => _isEditing;
		set => SetField(ref _isEditing, value);
	}

	public AsyncCommandBase LoadRepairRequestById { get; }
	public ICommand NavigateBackCommand { get; }
	public ICommand EditCommand { get; }
	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
	public ICommand DeleteCommand { get; }

	private void OnEditExecuted(object? p)
	{
		IsEditing = true;
		_tempRepairRequest = new RepairRequestListItemViewModel(RepairRequest.GetRepairRequest());
	}

	private void OnCancelExecuted(object? p)
	{
		IsEditing = false;
		RepairRequest = _tempRepairRequest;
	}

	public void UpdateRepairRequest(Domain.Models.RepairRequest repairRequest)
	{
		RepairRequest = new RepairRequestListItemViewModel(repairRequest);
		IsLoading = false;
	}

	private readonly ObservableCollection<RepairRequestStatus> _repairRequestStatuses;
	public IEnumerable<RepairRequestStatus> RepairRequestStatuses => _repairRequestStatuses;
}