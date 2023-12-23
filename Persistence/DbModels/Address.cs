using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.DbModels;

public partial class Address
{
    public int Id { get; set; }
    public string Country { get; set; } = null!;
    public string Region { get; set; } = null!;
    public string City { get; set; } = null!;
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string PostCode { get; set; } = null!;
    [NotMapped]
    public virtual CustomerAddress? CustomerAddress { get; set; }
    [NotMapped]
	public virtual Factory? Factory { get; set; }
}
