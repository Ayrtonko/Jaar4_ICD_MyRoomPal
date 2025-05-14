using Microsoft.EntityFrameworkCore;
using myroompal_api.Data;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.RoomManagement.Interfaces;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.RoomManagement.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _context;

    public RoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskResult<List<Room>>> GetAllRoomsAsync()
    {
        try
        {
            List<Room> rooms = await _context.Rooms.ToListAsync();
            return TaskResult<List<Room>>.Success(rooms);
        }
        catch (Exception e)
        {
            return TaskResult<List<Room>>.Failure("Could not retrieve any rooms. " + e.Message);
        }
    }

    public async Task<TaskResult<Room>> CreateRoomAsync(Room room)
    {
        try
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            return TaskResult<Room>.Success(room);
        }
        catch (Exception e)
        {
            return TaskResult<Room>.Failure("Could not create a room. " + e.Message);
        }
    }

    public Task<TaskResult<Room>> GetRoomByIdAsync(Guid id)
    {
        try
        {
            Room? room = _context.Rooms
                .Include(r => r.Address)
                .FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return Task.FromResult(TaskResult<Room>.Failure("Room not found."));
            }

            return Task.FromResult(TaskResult<Room>.Success(room));
        }
        catch (Exception e)
        {
            return Task.FromResult(TaskResult<Room>.Failure("Could not retrieve the room. " + e.Message));
        }
    }
}