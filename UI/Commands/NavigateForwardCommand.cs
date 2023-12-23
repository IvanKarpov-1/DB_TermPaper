using UI.Commands.Base;
using UI.Stores;

namespace UI.Commands;

public class NavigateForwardCommand : CommandBase
{
	private readonly NavigationStore _navigationStore;

	public NavigateForwardCommand(NavigationStore navigationStore)
	{
		_navigationStore = navigationStore;
		_navigationStore.CanNavigateForwardChanged += OnCanExecutedChanged;
	}

	public override void Execute(object? parameter)
	{
		_navigationStore.NavigateForward();
	}

	public override bool CanExecute(object? parameter)
	{
		return _navigationStore.CanNavigateForward;
	}
}