using System.Windows.Controls;
using UI.ValidationRules;
using UI.ViewModels.Base;

namespace UI.ViewModels.Factory;

public class FactoryListItemViewModel : ViewModelBaseWithValidation, ISelectableListItem, IEquatable<FactoryListItemViewModel>
{
	private bool _isSelected;
	public bool IsSelected
	{
		get => _isSelected;
		set => SetField(ref _isSelected, value);
	}

	public FactoryListItemViewModel(Domain.Models.Factory factory)
	{
		Factory = factory;

		ValidationRulesDictionary = new Dictionary<string, List<ValidationRule>?>
		{
			{ 
				nameof(Email), 
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(100)
				}
			},
			{ 
				nameof(Phone), 
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(16)
				}
			},
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

	public Domain.Models.Factory Factory { get; }

	public int Id
	{
		get => Factory.Id;
		set
		{
			Factory.Id = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Email
	{
		get => Factory.Email;
		set
		{
			Factory.Email = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Phone
	{
		get => Factory.Phone;
		set
		{
			Factory.Phone = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Country
	{
		get => Factory.Address.Country;
		set
		{
			Factory.Address.Country = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Region
	{
		get => Factory.Address.Region;
		set
		{
			Factory.Address.Region = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string City
	{
		get => Factory.Address.City;
		set
		{
			Factory.Address.City = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string AddressLine1
	{
		get => Factory.Address.AddressLine1;
		set
		{
			Factory.Address.AddressLine1 = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string AddressLine2
	{
		get => Factory.Address.AddressLine2 ?? "";
		set
		{
			Factory.Address.AddressLine2 = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string PostCode
	{
		get => Factory.Address.PostCode;
		set
		{
			Factory.Address.PostCode = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string AddressString => Factory.Address.ToString();

	public bool Equals(FactoryListItemViewModel? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Factory.Equals(other.Factory);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((FactoryListItemViewModel)obj);
	}

	public override int GetHashCode()
	{
		return Factory.GetHashCode();
	}
}