using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.DbModels;

public partial class Factory
{
    public int Id { get; set; }
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int AddressId { get; set; }
    [NotMapped]
	public virtual Address Address { get; set; } = null!;
	[NotMapped]
	public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
