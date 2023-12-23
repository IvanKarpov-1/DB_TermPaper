namespace Domain.Models;

public class Factory : IEquatable<Factory>
{
	public int Id { get; set; }
	public string Phone { get; set; } = "";
	public string Email { get; set; } = "";
	public int AddressId { get; set; }
	public Address Address { get; set; } = new();

	public bool Equals(Factory? other)
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
		return Equals((Factory)obj);
	}

	public override int GetHashCode()
	{
		return Id;
	}
}