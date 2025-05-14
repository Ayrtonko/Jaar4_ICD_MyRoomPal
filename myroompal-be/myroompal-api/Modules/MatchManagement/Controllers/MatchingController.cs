using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myroompal_api.Modules.MatchManagement.Interfaces;
using myroompal_api.Modules.MatchManagement.Models;
using myroompal_api.Modules.MatchManagement.Models.Requests;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.MatchManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchingController : ControllerBase
{
    private readonly IMatchingService _matchingService;
    private readonly IAuth0Context _iAuth0Context;

    public MatchingController(IMatchingService matchingService, IAuth0Context iAuth0Context)
    {
        _matchingService = matchingService;
        _iAuth0Context = iAuth0Context;
    }

    [HttpGet("unliked-profiles")]
    [Authorize]
    public async Task<ActionResult<List<UnlikedProfileVm>>> GetUnlikedProfiles()
    {
        try
        {
            var loggedInAuthId = _iAuth0Context.GetAuth0Id(User);
            TaskResult<List<UnlikedProfileVm>> result = await _matchingService.GetUnlikedProfiles(loggedInAuthId);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("delete-match")]
    [Authorize]
    public async Task<ActionResult> DeleteMatch([FromBody]DeleteMatchRequest request)
    {
        try
        {
            var loggedInAuthId = _iAuth0Context.GetAuth0Id(User);
            var result = await _matchingService.DeleteMatch(loggedInAuthId, request.LikeeUserId);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpPost("create-like")]
    [Authorize]
    public async Task<ActionResult> CreateLike(Guid likeeUserId)
    {
        try
        {
            var loggedInAuthId = _iAuth0Context.GetAuth0Id(User);
            TaskResult<ActionResult> result = await _matchingService.CreateLike(loggedInAuthId, likeeUserId);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("user-match-profiles")]
    [Authorize]
    public async Task<ActionResult<List<ProfileVm>>> GetUserMatchProfiles()
    {
        try
        {
            var loggedInAuthId = _iAuth0Context.GetAuth0Id(User);
            TaskResult<List<ProfileVm>> result = await _matchingService.GetUserMatchProfiles(loggedInAuthId);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("preferences")]
    [Authorize]
    public async Task<ActionResult<List<PreferenceVm>>> GetPreferences()
    {
        try
        {
            TaskResult<List<PreferenceVm>> result = await _matchingService.GetPreferences();
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("logged-user-preferences")]
    [Authorize]
    public async Task<ActionResult<List<PreferenceVm>>> GetLoggedUserPreferences()
    {
        try
        {
            var loggedInAuthId = _iAuth0Context.GetAuth0Id(User);
            TaskResult<List<PreferenceVm>> result = await _matchingService.GetLoggedUserPreferences(loggedInAuthId);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("create-preferences-user")]
    [Authorize]
    public async Task<ActionResult> CreatePreferencesUser([FromBody] PreferenceUserRequest request)
    {
        try
        {
            var loggedInAuthId = _iAuth0Context.GetAuth0Id(User);
            TaskResult<ActionResult> result = await _matchingService.CreatePreferencesUser(request, loggedInAuthId);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("update-profile-searchlocation")]
    [Authorize]
    public async Task<ActionResult> UpdateSearchLocation([FromBody] SearchLocationRequest request)
    {
        try
        {
            var loggedInAuthId = _iAuth0Context.GetAuth0Id(User);

            TaskResult<ActionResult> result = await _matchingService.UpdateUserSearchLocation(request, loggedInAuthId);
            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}