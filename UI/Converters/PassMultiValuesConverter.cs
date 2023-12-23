using System.Globalization;
using System.Windows.Data;

namespace UI.Converters;

public class PassMultiValuesConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		var parameterValuePairs = values.Select((value, index) => new { Name = $"p{index + 1}", Value = value });

		return parameterValuePairs.ToDictionary(pair => pair.Name, pair => pair.Value);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}