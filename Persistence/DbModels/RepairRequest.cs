using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.DbModels;

public partial class RepairRequest
{
    public int Id { get; set; }
    public DateOnly RequestDate { get; set; }
    public int StatusId { get; set; }
    public string? Description { get; set; }
    public int? CustomerId { get; set; }
    [NotMapped]
	public virtual Customer? Customer { get; set; }
	[NotMapped]
	public virtual RepairRequestStatus Status { get; set; } = null!;
}
