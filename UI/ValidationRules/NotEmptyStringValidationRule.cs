using System.Globalization;
using System.Windows.Controls;

namespace UI.ValidationRules;

class NotEmptyStringValidationRule : ValidationRule
{
	public override ValidationResult Validate(object value, CultureInfo cultureInfo)
	{
		return string.IsNullOrWhiteSpace(value.ToString())
			? new ValidationResult(false, "Поле обов'язкове для заповнення.")
			: ValidationResult.ValidResult;
	}
}