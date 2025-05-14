using Microsoft.EntityFrameworkCore;

namespace myroompal_api.Entities.Entities;

public class Room
{
    public Guid Id { get; set; }
    public required String RoomName { get; set; }
    public required String Description { get; set; }
    public required String ImageLink { get; set; }

    [Precision(10,2)]
    public required decimal RentPrice { get; set; }
    public required int Size { get; set; }
    public required bool Status { get; set; } = false;

    public Guid OwnerId { get; set; }
    public User? Owner { get; set; }

    public Guid? TenantId { get; set; }
    public User? Tenant { get; set; }

    public Guid AddressId { get; set; }
    public Address? Address { get; set; }

    public ICollection<Rent> Rents { get; } = new List<Rent>();
}