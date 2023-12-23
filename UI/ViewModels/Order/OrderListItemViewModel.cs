using System.Windows.Controls;
using UI.ViewModels.Base;

namespace UI.ViewModels.Order;

public class OrderListItemViewModel : ViewModelBaseWithValidation, ISelectableListItem
{
	private bool _isSelected;

	public bool IsSelected
	{
		get => _isSelected;
		set => SetField(ref _isSelected, value);
	}

	public OrderListItemViewModel(Domain.Models.Order order, string? address)
	{
		Order = order;
		Address = address;
		ValidationRulesDictionary = new Dictionary<string, List<ValidationRule>?>();
	}

	public Domain.Models.Order Order { get; }

	public int Id
	{
		get => Order.Id;
		set
		{
			Order.Id = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public DateOnly OrderDate
	{
		get => Order.OrderDate;
		set
		{
			Order.OrderDate = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public DateOnly DeliveryDate
	{
		get => Order.DeliveryDate;
		set
		{
			Order.DeliveryDate = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string? Address { get; }

	public Domain.Models.Order GetOrder()
	{
		return new Domain.Models.Order
		{
			Id = Id,
			OrderDate = OrderDate,
			DeliveryDate = DeliveryDate,
			CustomerId = Order.CustomerId,
			AddressId = Order.AddressId,
			OrderDetails = Order.OrderDetails
		};
	}
}