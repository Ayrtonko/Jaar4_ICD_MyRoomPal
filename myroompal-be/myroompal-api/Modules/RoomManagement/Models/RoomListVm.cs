using myroompal_api.Entities.Entities;

namespace myroompal_api.Modules.RoomManagement.Models;

public class RoomListVm
{
    public Guid Id { get; set; }
    public String RoomName { get; set; }
    public decimal RentPrice { get; set; }
    public String ImageLink { get; set; }

    public static RoomListVm CreateRoomListVm(Room room)
    {
        return new RoomListVm
        {
            Id = room.Id,
            RoomName = room.RoomName,
            RentPrice = room.RentPrice,
            ImageLink = room.ImageLink
        };
    }
}