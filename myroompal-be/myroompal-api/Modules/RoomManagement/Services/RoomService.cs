using myroompal_api.Entities.Entities;
using myroompal_api.Modules.RoomManagement.Interfaces;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.RoomManagement.Services;


public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<TaskResult<List<Room>>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAllRoomsAsync();
    }

    public async Task<TaskResult<Room>> CreateRoomAsync(Room room)
    {
        return await _roomRepository.CreateRoomAsync(room);
    }

    public async Task<TaskResult<Room>> GetRoomByIdAsync(Guid id)
    {
        return await _roomRepository.GetRoomByIdAsync(id);
    }
}