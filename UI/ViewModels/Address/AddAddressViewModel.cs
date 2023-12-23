using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using UI.Commands;
using UI.Commands.Address;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.ViewModels.Address;

public class AddAddressViewModel : ViewModelBase
{
	public AddAddressViewModel(
		int customerId,
		NavigationStore navigationStore,
		AddressStore addressStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		CustomerId = customerId;
		_address = new AddressListItemViewModel(new Domain.Models.Address());

		SaveCommand = new AddAddressCommand(this, addressStore, snackbarMessageQueue);
		CancelCommand = new NavigateBackCommand(navigationStore);
	}

	public static AddAddressViewModel LoadViewModel(
		int customerId,
		NavigationStore navigationStore,
		AddressStore addressStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		var viewModel = new AddAddressViewModel(
			customerId,
			navigationStore,
			addressStore,
			snackbarMessageQueue);

		return viewModel;
	}

	public int CustomerId { get; }

	private AddressListItemViewModel _address;
	public AddressListItemViewModel Address
	{
		get => _address;
		set => SetField(ref _address, value);
	}

	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
}