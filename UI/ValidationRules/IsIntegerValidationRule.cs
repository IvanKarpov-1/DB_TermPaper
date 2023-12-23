using System.Globalization;
using System.Windows.Controls;

namespace UI.ValidationRules;

public class IsIntegerValidationRule : ValidationRule
{
	public override ValidationResult Validate(object value, CultureInfo cultureInfo)
	{
		if (value.ToString() == null) return new ValidationResult(false, "Поле не повинно бути порожнім");

		var result = int.TryParse(value.ToString(), out _);
		
		return result == false
			? new ValidationResult(false, "Невірний формат цілого числа")
			: ValidationResult.ValidResult;
	}
}