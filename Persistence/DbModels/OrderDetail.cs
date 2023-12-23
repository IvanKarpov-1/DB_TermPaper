using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.DbModels;

public partial class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int? ProductId { get; set; }
    public decimal TotalPrice { get; set; }
    public int Quantity { get; set; }
    [NotMapped]
	public virtual Order Order { get; set; } = null!;
	[NotMapped]
	public virtual Product? Product { get; set; }
}
