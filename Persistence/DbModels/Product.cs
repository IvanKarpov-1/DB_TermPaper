using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.DbModels;

public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Code { get; set; }
    public string YearOfLaunch { get; set; } = null!;
    public int NumberPerYear { get; set; }
    public string TechSpecs { get; set; } = null!;
    public decimal SellingPrice { get; set; }
    [NotMapped]
	public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    [NotMapped]
	public virtual ICollection<Factory> Factories { get; set; } = new List<Factory>();
}
