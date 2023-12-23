using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.DbModels;

public partial class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    [NotMapped]
	public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();
	[NotMapped]
	public virtual ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}
