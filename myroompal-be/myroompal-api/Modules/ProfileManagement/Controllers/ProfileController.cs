using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myroompal_api.Modules.ProfileManagement.Models;
using myroompal_api.Modules.Shared;
using myroompal_api.Modules.UserManagement.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IUserService _userService;
    private readonly IAuth0Context _auth0Context;

    public ProfileController(IProfileService profileService, IAuth0Context auth0Context, IUserService userService)
    {
        _profileService = profileService;
        _auth0Context = auth0Context;
        _userService = userService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateProfile([FromBody] ProfilePreferencesVm profilePreferencesVm)
    {
        try
        {
            var auth0Id = _auth0Context.GetAuth0Id(User);

            if(auth0Id == null){
                return BadRequest("Cannot find user");
            }
            var userid = await _userService.GetUserById(auth0Id);

              if(!userid.IsSuccessful){
                return BadRequest("Cannot find user");
            }
                profilePreferencesVm.UserId = userid.Result;
            if (userid.Result != profilePreferencesVm.UserId)
            {
                return Unauthorized("You are not authorized to modify this profile.");
            }
            
            var result = await _profileService.CreateOrUpdateProfile(profilePreferencesVm);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error in CreateOrUpdateProfile: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


    // GET: api/Profile
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ProfilePageVm>> GetProfile()
    {
        try
        {
            //Get auth id from token
            var auth0Id = _auth0Context.GetAuth0Id(User);

            //get profile by auth id
            var result = await _profileService.GetProfileByAuth0Id(auth0Id);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Result);
        }
        catch (Exception ex)
        {
            return BadRequest("Something went wrong while fetching the profile.");
        }
    }
}