using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Customer;
using UI.Services;
using UI.Stores;
using UI.ViewModels.Address;
using UI.ViewModels.Base;

namespace UI.ViewModels.Customer;

public class AddCustomerViewModel : ViewModelBase
{
	public AddCustomerViewModel(
		CustomerStore customerStore,
		NavigationService<CustomerListViewModel> customerListViewNavigationService,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_customer = new CustomerListItemViewModel(new Domain.Models.Customer());
		_customerAddress = new AddressListItemViewModel(new Domain.Models.Address());

		SaveCommand = new AddCustomerCommand(this, customerStore, snackbarMessageQueue);
		CancelCommand = new NavigateCommand<CustomerListViewModel>(customerListViewNavigationService);
	}

	private CustomerListItemViewModel _customer;
	public CustomerListItemViewModel Customer
	{
		get => _customer;
		set => SetField(ref _customer, value);
	}

	private AddressListItemViewModel _customerAddress;
	public AddressListItemViewModel CustomerAddress
	{
		get => _customerAddress;
		set => SetField(ref _customerAddress, value);
	}

	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
}