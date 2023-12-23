namespace Domain.Models;

public class Address : IEquatable<Address>
{
	public int Id { get; set; }
	public string Country { get; set; } = "";
	public string Region { get; set; } = "";
	public string City { get; set; } = "";
	public string AddressLine1 { get; set; } = "";
	public string? AddressLine2 { get; set; }
	public string PostCode { get; set; } = "";

	public string AddressString => ToString();

	public override string ToString()
	{
		return $"{Country}, {Region}, {City}, {AddressLine1} ({AddressLine2}), {PostCode}";
	}

	public bool Equals(Address? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Id == other.Id;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Address)obj);
	}

	public override int GetHashCode()
	{
		return Id;
	}
}