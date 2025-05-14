using myroompal_api.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using myroompal_api.Data;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using myroompal_api.Entities.Types;
using Microsoft.EntityFrameworkCore;
using myroompal_api.Modules.UserManagement.Interfaces;
using myroompal_api.Modules.UserManagement.Models;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.UserManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuth0Context _iAuth0Context;

        public UserController(IUserService userService, IAuth0Context iAuth0Context)
        {
            _userService = userService;
            _iAuth0Context = iAuth0Context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> GetUsers()
        {
            TaskResult<List<User>> result = await _userService.GetUsers();
            if(!result.IsSuccessful)
            {
                return BadRequest(TaskResult<List<User>>.Failure(result.Message));
            }
            return Ok(result.Result);
        }

        [HttpGet("get-user-id")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserById()
        {
            var user = _iAuth0Context.GetAuth0Id(User);
            var result = await _userService.GetUserById(user);
            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            return Ok(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserVm userVm)
        {
            if(userVm == null)
            {
                return BadRequest("User data is required.");
            }
            
            try 
            {
                var user = await _userService.CreateUser(userVm);
                if(!user.IsSuccessful)
                    return BadRequest(user.Message);

                return Ok(user.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<List<UserSearchVm>>> GetUsersBySearchQuery([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    return BadRequest("A query parameter is required.");
                }

                var result = await _userService.GetUserBySearchQuery(query);
                if (!result.IsSuccessful)
                    return BadRequest(result.Message);

                return Ok(result.Result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        

        [HttpPut("status")]
        [Authorize]
        public async Task<IActionResult> UpdateUserStatus([FromBody] UserSearchVm userSearchVm)
        {
            try
            {
                if (userSearchVm.Id == Guid.Empty ||
                    userSearchVm.Id == null ||
                    userSearchVm.Status == null)
                    return BadRequest("Id and Status are required.");

                var result = await _userService.UpdateUserStatus(userSearchVm);
                if (!result.IsSuccessful)
                    return BadRequest(result.Message);
                
                return Ok(result.Result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("user-role")]
        [Authorize]
        public async Task<IActionResult> GetUserRole()
        {
            try
            {
                var user = _iAuth0Context.GetAuth0Id(User);
                var result = await _userService.GetUserRole(user);
                if (!result.IsSuccessful)
                    return BadRequest(result.Message);

                return Ok(result.Result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}