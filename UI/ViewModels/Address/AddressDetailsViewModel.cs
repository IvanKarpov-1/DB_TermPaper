using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Address;
using UI.Commands.Base;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.ViewModels.Address;

public class AddressDetailsViewModel : ViewModelBase, ILoadingViewModel, IEditableViewModel
{
	private readonly int _addressId;

	public AddressDetailsViewModel(
		int addressId,
		NavigationStore navigationStore,
		AddressStore addressStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_addressId = addressId;
		LoadAddressCommand = new LoadAddressByIdCommand(this, addressStore);
		NavigateBackCommand = new NavigateBackCommand(navigationStore);
		EditCommand = new RelayCommand(OnEditExecuted, _ => true);
		SaveCommand = new UpdateAddressCommand(this, addressStore, snackbarMessageQueue);
		CancelCommand = new RelayCommand(OnCancelExecuted, _ => true);
		DeleteCommand = new DeleteAddressCommand(this, addressStore, navigationStore, snackbarMessageQueue);
	}

	public static AddressDetailsViewModel LoadViewModel(
		int addressId,
		NavigationStore navigationStore,
		AddressStore addressStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		var viewModel = new AddressDetailsViewModel(
			addressId,
			navigationStore,
			addressStore,
			snackbarMessageQueue);
		viewModel.Load();
		return viewModel;
	}

	public async void Load()
	{
		await LoadAddressCommand.ExecuteAsync(_addressId);
	}

	private AddressListItemViewModel _address = null!;
	private AddressListItemViewModel _tempAddress = null!;
	public AddressListItemViewModel Address
	{
		get => _address;
		set => SetField(ref _address, value);
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

	public AsyncCommandBase LoadAddressCommand { get; }
	public ICommand NavigateBackCommand { get; }
	public ICommand EditCommand { get; }
	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
	public ICommand DeleteCommand { get; }

	private void OnEditExecuted(object? p)
	{
		IsEditing = true;
		_tempAddress = new AddressListItemViewModel(Address.Address);
	}

	private void OnCancelExecuted(object? p)
	{
		IsEditing = false;
		Address = _tempAddress;
	}

	public void UpdateAddress(Domain.Models.Address address)
	{
		Address = new AddressListItemViewModel(address);
		IsLoading = false;
	}
}