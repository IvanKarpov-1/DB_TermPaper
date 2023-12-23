using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.DbModels;

public partial class RepairRequestStatus
{
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;
	[NotMapped]
	public virtual ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}
