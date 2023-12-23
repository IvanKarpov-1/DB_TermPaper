using System.Windows.Controls;
using UI.ValidationRules;
using UI.ViewModels.Base;

namespace UI.ViewModels.Product;

public class ProductListItemViewModel : ViewModelBaseWithValidation, ISelectableListItem
{
	private bool _isSelected;
	public bool IsSelected
	{
		get => _isSelected;
		set => SetField(ref _isSelected, value);
	}

	public ProductListItemViewModel(Domain.Models.Product product)
	{
		Product = product;

		ValidationRulesDictionary = new Dictionary<string, List<ValidationRule>?>
		{
			{
				nameof(Name),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(50)
				}
			},
			{
				nameof(Model),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(20)
				}
			},
			{
				nameof(Code),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new IsIntegerValidationRule()
				}
			},
			{
				nameof(NumberPerYear),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new IsIntegerValidationRule()
				}
			},
			{
				nameof(YearOfLaunch),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(4)
				}
			},
			{
				nameof(TechSpecs), 
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule()
				}
			},
			{
				nameof(SellingPrice),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new IsDecimalValidationRule()
				}
			}
		};
	}

	public Domain.Models.Product Product { get; }

	public int Id
	{
		get => Product.Id;
		set
		{
			Product.Id = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Name
	{
		get => Product.Name;
		set
		{
			Product.Name = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Model
	{
		get => Product.Model;
		set
		{
			Product.Model = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}
	public string Code
	{
		get => Product.Code.ToString() ?? "";
		set {
			if (ValidateProperty(value) == false)
			{
				if (value.Length == 0)
					Product.Code = null;
				return;
			}

			int.TryParse(value, out var intValue);
			Product.Code = intValue;
			OnPropertyChanged();
		}
	}
	public string YearOfLaunch
	{
		get => Product.YearOfLaunch;
		set
		{
			Product.YearOfLaunch = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}
	public string NumberPerYear
	{
		get => Product.NumberPerYear.ToString() ?? "";
		set
		{
			if (ValidateProperty(value) == false)
			{
				if (value.Length == 0)
					Product.NumberPerYear = null;
				return;
			}

			int.TryParse(value, out var intValue);
			Product.NumberPerYear = intValue;
			OnPropertyChanged();
		}
	}
	public string TechSpecs
	{
		get => Product.TechSpecs;
		set
		{
			Product.TechSpecs = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string SellingPrice
	{
		get => Product.SellingPrice.ToString() ?? "";
		set
		{
			if (ValidateProperty(value) == false)
			{
				if (value.Length == 0) 
					Product.SellingPrice = null;
				return;
			}
			
			decimal.TryParse(value, out var decimalValue);

			Product.SellingPrice = decimalValue;
			OnPropertyChanged();
		}
	}

	public Domain.Models.Product GetProduct()
	{
		int.TryParse(Code, out var intCode);
		int.TryParse(NumberPerYear, out var intNumberPerYear);
		decimal.TryParse(SellingPrice, out var decimalSellingPrice);

		return new Domain.Models.Product
		{
			Id = Id,
			Code = intCode,
			Model = Model,
			Name = Name,
			NumberPerYear = intNumberPerYear,
			SellingPrice = decimalSellingPrice,
			TechSpecs = TechSpecs,
			YearOfLaunch = YearOfLaunch
		};
	}
}