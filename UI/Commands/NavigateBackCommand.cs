using UI.Commands.Base;
using UI.Stores;

namespace UI.Commands;

public class NavigateBackCommand : CommandBase
{
	private readonly NavigationStore _navigationStore;

	public NavigateBackCommand(NavigationStore navigationStore)
	{
		_navigationStore = navigationStore;
		_navigationStore.CanNavigateBackwardChanged += OnCanExecutedChanged;
	}

	public override void Execute(object? parameter)
	{
		_navigationStore.NavigateBack();
	}

	public override bool CanExecute(object? parameter)
	{
		return _navigationStore.CanNavigateBackward;
	}
}