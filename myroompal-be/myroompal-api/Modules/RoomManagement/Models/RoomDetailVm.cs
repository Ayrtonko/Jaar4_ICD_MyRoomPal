using myroompal_api.Entities.Entities;

namespace myroompal_api.Modules.RoomManagement.Models;

public class RoomDetailVm
{
    public Guid Id { get; set; }
    public String RoomName { get; set; }
    public decimal RentPrice { get; set; }
    public String ImageLink { get; set; }
    public String Description { get; set; }
    public int Size { get; set; }
    public Address Address { get; set; }

    public static RoomDetailVm CreateRoomDetailVm(Room room)
    {
        return new RoomDetailVm
        {
            Id = room.Id,
            RoomName = room.RoomName,
            RentPrice = room.RentPrice,
            ImageLink = room.ImageLink,
            Description = room.Description,
            Address = room.Address,
            Size = room.Size
        };
    }
}