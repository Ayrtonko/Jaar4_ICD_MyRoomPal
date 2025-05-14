using myroompal_api.Entities.Entities;

namespace myroompal_api.Modules.RoomManagement.Models;

public class RoomCreationVm
{
    public Guid? Id { get; set; }
    public String RoomName { get; set; }
    public String Description { get; set; }
    public String ImageLink { get; set; }
    public decimal RentPrice { get; set; }
    public int Size { get; set; }
    public bool? Status { get; set; } = false;
    public Guid? OwnerId { get; set; }
    public Guid? TenantId { get; set; } = null;
    public String Country { get; set; }
    public String City { get; set; }
    public String StreetName { get; set; }
    public String PostalCode { get; set; }

    public static RoomCreationVm CreateRoomCreationVm(Room room)
    {
        return new RoomCreationVm
        {
            Id = room.Id,
            RoomName = room.RoomName,
            Description = room.Description,
            ImageLink = room.ImageLink,
            RentPrice = room.RentPrice,
            Size = room.Size,
            Status = room.Status,
            OwnerId = room.OwnerId,
            TenantId = room.TenantId,
            Country = room.Address.Country,
            City = room.Address.City,
            StreetName = room.Address.StreetName,
            PostalCode = room.Address.PostalCode
        };
    }

    public Room ToEntity()
    {
        var adressId = Guid.NewGuid();
        return new Room
        {
            Id = Guid.NewGuid(),
            RoomName = RoomName,
            Description = Description,
            ImageLink = ImageLink,
            RentPrice = RentPrice,
            Size = Size,
            Status = Status ?? false,
            OwnerId = OwnerId ?? Guid.Empty,
            TenantId = TenantId,
            AddressId = adressId,
            Address = new Address
            {
                Id = adressId,
                Country = Country,
                City = City,
                StreetName = StreetName,
                PostalCode = PostalCode
            }
        };
    }
}