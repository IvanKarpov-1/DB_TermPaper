using System.Windows.Input;

namespace UI.Commands.Base;

public abstract class CommandBase : ICommand
{
	public event EventHandler? CanExecuteChanged;

	public virtual bool CanExecute(object? parameter)
	{
		return true;
	}

	public abstract void Execute(object? parameter);

	protected virtual void OnCanExecutedChanged()
	{
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}