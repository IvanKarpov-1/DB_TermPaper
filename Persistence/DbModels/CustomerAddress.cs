using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.DbModels;

public partial class CustomerAddress
{
    public int CustomerId { get; set; }
    public int AddressId { get; set; }
    [NotMapped]
	public virtual Address Address { get; set; } = null!;
	[NotMapped]
	public virtual Customer Customer { get; set; } = null!;
	[NotMapped]
	public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
