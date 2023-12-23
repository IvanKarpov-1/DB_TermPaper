using UI.Commands.Base;
using UI.Services;
using UI.ViewModels.Base;

namespace UI.Commands;

public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
{
	private readonly NavigationService<TViewModel> _navigationService;

	public NavigateCommand(NavigationService<TViewModel> navigationService)
	{
		_navigationService = navigationService;
	}

	public override void Execute(object? parameter)
	{
		_navigationService.Navigate(parameter);
	}
}