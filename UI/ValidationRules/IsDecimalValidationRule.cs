using System.Globalization;
using System.Windows.Controls;

namespace UI.ValidationRules;

public class IsDecimalValidationRule : ValidationRule
{
	public override ValidationResult Validate(object value, CultureInfo cultureInfo)
	{
		if (value.ToString() == null) return new ValidationResult(false, "Поле не повинно бути порожнім");
		
		var culture = CultureInfo.CurrentCulture;
		var result = decimal.TryParse(value.ToString(), culture, out _);

		return result == false 
			? new ValidationResult(false, "Невірний формат дійсного числа") 
			: ValidationResult.ValidResult;
	}
}