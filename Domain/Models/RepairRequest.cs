namespace Domain.Models;

public class RepairRequest
{
	public int Id { get; set; }
	public DateOnly RequestDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
	public int StatusId { get; set; } = RepairRequestStatus.New.Id;
	public string StatusName { get; set; } = RepairRequestStatus.New.DisplayName;

	public string? Description { get; set; }
	public int? CustomerId { get; set; }
}