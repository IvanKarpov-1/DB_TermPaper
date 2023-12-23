using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace UI.ViewModels.Base;

public class ViewModelBaseWithValidation : ViewModelBase, INotifyDataErrorInfo
{
	protected Dictionary<string, List<ValidationRule>?>? ValidationRulesDictionary;
	private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;
	private readonly object _syncLock = new();

	protected ViewModelBaseWithValidation()
	{
		_propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
	}

	private bool _hasError;
	public bool HasErrors
	{
		get => _hasError;
		set => SetField(ref _hasError, value);
	}

	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

	public IEnumerable GetErrors(string? propertyName)
	{
		return _propertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
	}

	public bool ValidateProperty(object value, [CallerMemberName] string propertyName = null!)
	{
		lock (_syncLock)
		{
			if (ValidationRulesDictionary == null) return false;

			if (ValidationRulesDictionary.TryGetValue(propertyName, out var propertyValidationRules) == false)
				return false;

			if (_propertyNameToErrorsDictionary.ContainsKey(propertyName))
			{
				_propertyNameToErrorsDictionary.Remove(propertyName);
				OnErrorChanged(propertyName);
			}

			propertyValidationRules?.ForEach(validationRule =>
			{
				var result = validationRule.Validate(value, CultureInfo.CurrentCulture);
				if (result.IsValid == false)
				{
					AddError(propertyName, (string)result.ErrorContent, false);
				}
			});

			var result = !_propertyNameToErrorsDictionary.TryGetValue(propertyName, out var errors);
			if (errors != null) result = errors.Count == 0;
			return result;
		}
	}

	public void AddError(string propertyName, string error, bool isWarning)
	{
		if (_propertyNameToErrorsDictionary.ContainsKey(propertyName) == false)
		{
			_propertyNameToErrorsDictionary[propertyName] = new List<string>();
		}

		if (_propertyNameToErrorsDictionary[propertyName].Contains(error)) return;

		if (isWarning)
		{
			_propertyNameToErrorsDictionary[propertyName].Add(error);
		}
		else
		{
			_propertyNameToErrorsDictionary[propertyName].Insert(0, error);
		}

		OnErrorChanged(propertyName);
	}

	public void RemoveError(string propertyName, string error)
	{
		if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName) ||
		    !_propertyNameToErrorsDictionary[propertyName].Contains(error)) return;

		_propertyNameToErrorsDictionary[propertyName].Remove(error);

		if (_propertyNameToErrorsDictionary[propertyName].Count == 0)
		{
			_propertyNameToErrorsDictionary.Remove(propertyName);
		}

		OnErrorChanged(propertyName);
	}

	public void OnErrorChanged(string propertyName)
	{
		ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		HasErrors = _propertyNameToErrorsDictionary.Count > 0;
	}
}