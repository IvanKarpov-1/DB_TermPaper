namespace Domain.Models;

public class Order
{
	public int Id { get; set; }
	public int? CustomerId { get; set; }
	public int? AddressId { get; set; }
	public DateOnly OrderDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
	public DateOnly DeliveryDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
	public IEnumerable<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}