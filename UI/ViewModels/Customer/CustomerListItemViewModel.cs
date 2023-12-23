using System.Windows.Controls;
using UI.ValidationRules;
using UI.ViewModels.Base;

namespace UI.ViewModels.Customer;

public class CustomerListItemViewModel : ViewModelBaseWithValidation, ISelectableListItem
{
	private bool _isSelected;

	public bool IsSelected
	{
		get => _isSelected;
		set => SetField(ref _isSelected, value);
	}

	public CustomerListItemViewModel(Domain.Models.Customer customer)
	{
		Customer = customer;

		ValidationRulesDictionary = new Dictionary<string, List<ValidationRule>?>
		{
			{
				nameof(Name),
				new List<ValidationRule>
				{
					new NotEmptyStringValidationRule(),
					new LengthLessThanSpecifiedValidationRule(100)
				}
			},
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
			}
		};
	}

	public Domain.Models.Customer Customer { get; }

	public int Id
	{
		get => Customer.Id;
		set
		{
			Customer.Id = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Name
	{
		get => Customer.Name;
		set
		{
			Customer.Name = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Email
	{
		get => Customer.Email;
		set
		{
			Customer.Email = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string Phone
	{
		get => Customer.Phone;
		set
		{
			Customer.Phone = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}
}