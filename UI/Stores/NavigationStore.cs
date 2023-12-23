using UI.ViewModels.Base;

namespace UI.Stores;

public class NavigationStore
{
	private readonly Stack<(Func<object?, ViewModelBase>?, object?)> _history;
	private readonly Stack<(Func<object?, ViewModelBase>?, object?)> _forwardHistory;

	public event Action? CurrentViewModelChanged;
	public event Action? CanNavigateBackwardChanged;
	public event Action? CanNavigateForwardChanged;

	public NavigationStore()
	{
		_history = new Stack<(Func<object?, ViewModelBase>?, object?)>();
		_forwardHistory = new Stack<(Func<object?, ViewModelBase>?, object?)>();
	}

	public void AddViewModelCreateFunc(Func<object?, ViewModelBase> createViewModel, object? parameter)
	{
		ClearForwardHistory();
		_history.Push((createViewModel, parameter));
		OnCurrentViewModelChanged();
	}

	public ViewModelBase? CurrentViewModel
	{
		get
		{
			if (_history.Count == 0) return null;
			var (createViewModel, parameter) = _history.Peek();
			UpdateCanNavigate();
			return createViewModel?.Invoke(parameter);
		}
	}

	public bool CanNavigateBackward { get; private set; }
	public bool CanNavigateForward { get; private set; }

	private void OnCurrentViewModelChanged()
	{
		CurrentViewModelChanged?.Invoke();
	}

	public void NavigateBack()
	{
		if (_history.Count <= 1) return;

		_forwardHistory.Push(_history.Pop());
		OnCurrentViewModelChanged();
	}

	public void NavigateForward()
	{
		if (CanNavigateForward == false) return;

		_history.Push(_forwardHistory.Pop());
		OnCurrentViewModelChanged();
	}

	public void ClearForwardHistory()
	{
		_forwardHistory.Clear();
	}

	private void UpdateCanNavigate()
	{
		CanNavigateBackward = _history.Count > 1;
		CanNavigateBackwardChanged?.Invoke();
		CanNavigateForward = _forwardHistory.Count > 0;
		CanNavigateForwardChanged?.Invoke();
	}
}