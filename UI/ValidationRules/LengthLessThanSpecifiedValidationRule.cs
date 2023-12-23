using System.Globalization;
using System.Windows.Controls;

namespace UI.ValidationRules;

public class LengthLessThanSpecifiedValidationRule : ValidationRule
{
	private int _maxLength;

	public LengthLessThanSpecifiedValidationRule(int maxLength)
	{
		_maxLength = maxLength;
	}

	public override ValidationResult Validate(object value, CultureInfo cultureInfo)
	{
		return value.ToString()?.Length > _maxLength
			? new ValidationResult(false, $"Поле не повинно бути довше за {_maxLength} символів")
			: ValidationResult.ValidResult;
	}
}