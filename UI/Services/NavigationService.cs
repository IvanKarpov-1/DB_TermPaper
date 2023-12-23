using UI.Stores;
using UI.ViewModels.Base;

namespace UI.Services;

public class NavigationService<TViewModel> where TViewModel : ViewModelBase
{
	private readonly NavigationStore _navigationStore;
	private readonly Func<object?, TViewModel> _createViewModel;

	public NavigationService(NavigationStore navigationStore, Func<object?, TViewModel> createViewModel)
	{
		_navigationStore = navigationStore;
		_createViewModel = createViewModel;
	}

	public void Navigate(object? parameter)
	{
		_navigationStore.AddViewModelCreateFunc(_createViewModel, parameter);
	}
}