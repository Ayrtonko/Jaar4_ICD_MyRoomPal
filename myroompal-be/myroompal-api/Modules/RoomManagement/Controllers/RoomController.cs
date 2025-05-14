using Microsoft.AspNetCore.Mvc;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.RoomManagement.Interfaces;
using myroompal_api.Modules.RoomManagement.Models;
using myroompal_api.Modules.Shared;
using myroompal_api.Modules.UserManagement.Interfaces;

namespace myroompal_api.Modules.RoomManagement.Controllers;


    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly IAuth0Context _auth;

        public RoomController(IRoomService roomService, IUserService userService,IAuth0Context auth)
        {
            _roomService = roomService;
            _userService = userService;
            _auth = auth;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDetailVm>> GetRoomByIdAsync(Guid id)
        {
            try
            {
                TaskResult<Room> result = await _roomService.GetRoomByIdAsync(id);
                if(!result.IsSuccessful)
                {
                    return BadRequest(TaskResult<Room>.Failure(result.Message));
                }
                return Ok(RoomDetailVm.CreateRoomDetailVm(result.Result));
            }
            catch (Exception ex)
            {
                return BadRequest(TaskResult<Room>.Failure("Something went wrong! Could not retrieve any properties." + ex.Message));
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<RoomListVm>> GetAllRoomsAsync()
        {
            try
            {
                TaskResult<List<Room>> result = await _roomService.GetAllRoomsAsync();
                if(!result.IsSuccessful)
                {
                    return BadRequest(TaskResult<List<Room>>.Failure(result.Message));
                }
                return Ok(result.Result.Select(RoomListVm.CreateRoomListVm).ToList());
            }   
            catch (Exception ex)
            {
                return BadRequest(TaskResult<List<Room>>.Failure("Something went wrong! Could not retrieve any properties." + ex.Message));
            }     
        }

        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoomAsync([FromBody] RoomCreationVm? roomCreationVm)
        {
            if(roomCreationVm == null ||
               roomCreationVm.RoomName == String.Empty ||
                roomCreationVm.RentPrice == 0)
            return BadRequest(TaskResult<RoomCreationVm>.Failure("Invalid room data. Could not create a room. "));

            try
            {
                var ownerId = await _userService.GetUserById(_auth.GetAuth0Id(User));
                if (!ownerId.IsSuccessful)
                {
                    return BadRequest(TaskResult<RoomCreationVm>.Failure("Owner not found! Could not create a room."));
                }

                roomCreationVm.OwnerId = ownerId.Result;
                TaskResult<Room> result = await _roomService.CreateRoomAsync(roomCreationVm.ToEntity());

                if(!result.IsSuccessful)
                {
                    return BadRequest(TaskResult<RoomCreationVm>.Failure(result.Message));
                }
                return Ok("Room created successfully!");
            }
            catch(Exception e)
            {
                return BadRequest(TaskResult<RoomCreationVm>.Failure("Something went wrong! Could not create a room." + e.Message));
            }
        }
    }
