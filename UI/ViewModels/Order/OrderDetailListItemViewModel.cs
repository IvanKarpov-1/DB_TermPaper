using System.Windows.Controls;
using UI.ViewModels.Product;

namespace UI.ViewModels.Order;

public class OrderDetailListItemViewModel : ProductListItemViewModel
{
	public OrderDetailListItemViewModel(Domain.Models.Product product, Domain.Models.OrderDetail orderDetail) : base(product)
	{
		OrderDetail = orderDetail;
		ValidationRulesDictionary = new Dictionary<string, List<ValidationRule>?>();
	}

	public Domain.Models.OrderDetail OrderDetail;

	public int Quantity
	{
		get => OrderDetail.Quantity;
		set
		{
			OrderDetail.Quantity = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}
}