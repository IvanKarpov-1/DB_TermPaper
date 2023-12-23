using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.DbModels;

public partial class Order
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public DateOnly OrderDate { get; set; }
    public DateOnly DeliveryDate { get; set; }
    public int? AddressId { get; set; }
	[NotMapped]
	public virtual CustomerAddress? CustomerAddress { get; set; }
	[NotMapped]
	public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
