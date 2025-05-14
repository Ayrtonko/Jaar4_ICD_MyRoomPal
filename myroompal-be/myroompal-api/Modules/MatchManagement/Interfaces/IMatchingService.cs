using Microsoft.AspNetCore.Mvc;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.MatchManagement.Models;
using myroompal_api.Modules.MatchManagement.Models.Requests;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.MatchManagement.Interfaces;

public interface IMatchingService
{
    Task<TaskResult<List<UnlikedProfileVm>>> GetUnlikedProfiles(string loggedInAuthId);
    Task<TaskResult<ActionResult>> CreateLike(string loggedInAuthId, Guid likeeUserId);
    Task<TaskResult<ActionResult>> DeleteMatch(string loggedInAuthId, Guid likeeUserId);

    Task<TaskResult<List<ProfileVm>>> GetUserMatchProfiles(string loggedInAuthId);
    Task<TaskResult<List<PreferenceVm>>> GetLoggedUserPreferences(string loggedInAuthId);

    Task<TaskResult<List<PreferenceVm>>> GetPreferences();
    Task<TaskResult<ActionResult>> CreatePreferencesUser(PreferenceUserRequest request, string loggedInAuthId);
    Task<TaskResult<ActionResult>> UpdateUserSearchLocation(SearchLocationRequest request, string loggedInAuthId);
}