using System.Windows.Controls;
using UI.ValidationRules;
using UI.ViewModels.Base;

namespace UI.ViewModels.Address;

public class AddressListItemViewModel : ViewModelBaseWithValidation, ISelectableListItem, IEquatable<AddressListItemViewModel>
{
	private bool _isSelected;

	public bool IsSelected
	{
		get => _isSelected;
		set => SetField(ref _isSelected, value);
	}

	public AddressListItemViewModel(Domain.Models.Address address)
	{
		Address = address;

		ValidationRulesDictionary = new Dictionary<string, List<ValidationRule>?>
		{
			{
				nameof(Country),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(2)
				}
			},
			{
				nameof(Region),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(35)
				}
			},
			{
				nameof(City),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(35)
				}
			},
			{
				nameof(AddressLine1),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(70)
				}
			},
			{
				nameof(AddressLine2),
				new List<ValidationRule>
				{
					new LengthLessThanSpecifiedValidationRule(70)
				}
			},
			{
				nameof(PostCode),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(16)
				}
			}
		};
	}

	public Domain.Models.Address Address { get; }

	public int Id
	{
		get => Address.Id;
		set
		{
			Address.Id = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Country
	{
		get => Address.Country;
		set
		{
			Address.Country = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Region
	{
		get => Address.Region; 
		set
		{
			Address.Region = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string City
	{
		get => Address.City;
		set
		{
			Address.City = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string AddressLine1
	{
		get => Address.AddressLine1;
		set
		{
			Address.AddressLine1 = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string AddressLine2
	{
		get => Address.AddressLine2 ?? "";
		set
		{
			Address.AddressLine2 = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string PostCode
	{
		get => Address.PostCode;
		set
		{
			Address.PostCode = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string AddressString => Address.ToString();

	public bool Equals(AddressListItemViewModel? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Address.Equals(other.Address);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((AddressListItemViewModel)obj);
	}

	public override int GetHashCode()
	{
		return Address.GetHashCode();
	}
}