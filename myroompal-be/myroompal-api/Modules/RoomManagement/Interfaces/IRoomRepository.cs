using Microsoft.AspNetCore.Mvc;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.RoomManagement.Models;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.RoomManagement.Interfaces;

public interface IRoomRepository
{
    Task<TaskResult<List<Room>>> GetAllRoomsAsync();
    Task<TaskResult<Room>> CreateRoomAsync(Room room);

    Task<TaskResult<Room>> GetRoomByIdAsync(Guid id);
}