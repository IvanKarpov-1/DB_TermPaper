namespace Domain.Models;

public class Product : IEquatable<Product>
{
	public int Id { get; set; }
	public string Name { get; set; } = "";
	public string Model { get; set; } = "";
	public int? Code { get; set; }
	public string YearOfLaunch { get; set; } = "";
	public int? NumberPerYear { get; set; }
	public string TechSpecs { get; set; } = "";
	public decimal? SellingPrice { get; set; }

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}

	public bool Equals(Product? other)
	{
		return other != null && Id.Equals(other.Id);
	}
}