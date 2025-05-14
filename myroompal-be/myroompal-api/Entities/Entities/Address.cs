namespace myroompal_api.Entities.Entities;

public class Address
{
    public Guid Id { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public required string StreetName { get; set; }
    public required string PostalCode { get; set; }

    public ICollection<User> Users { get; } = new List<User>();
}