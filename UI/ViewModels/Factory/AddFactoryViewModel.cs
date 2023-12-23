using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Factory;
using UI.Stores;
using UI.ViewModels.Base;

namespace UI.ViewModels.Factory;

public class AddFactoryViewModel : ViewModelBaseWithValidation
{
	public AddFactoryViewModel(
		NavigationStore navigationStore,
		FactoryStore factoryStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		_factory = new FactoryListItemViewModel(new Domain.Models.Factory());

		SaveCommand = new AddFactoryCommand(this, factoryStore, snackbarMessageQueue);
		CancelCommand = new NavigateBackCommand(navigationStore);
;	}

	private FactoryListItemViewModel _factory;
	public FactoryListItemViewModel Factory
	{
		get => _factory;
		set => SetField(ref _factory, value);
	}

	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }
}