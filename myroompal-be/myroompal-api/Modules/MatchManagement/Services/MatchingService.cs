using Microsoft.AspNetCore.Mvc;
using myroompal_api.Modules.MatchManagement.Interfaces;
using myroompal_api.Modules.MatchManagement.Models;
using myroompal_api.Modules.MatchManagement.Models.Requests;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.MatchManagement.Services;

public class MatchingService : IMatchingService

{
    private readonly IMatchingRepository _matchingRepository;

    public MatchingService(IMatchingRepository matchingRepository)
    {
        this._matchingRepository = matchingRepository;
    }

    public async Task<TaskResult<List<UnlikedProfileVm>>> GetUnlikedProfiles(string loggedInAuthId)
    {
        TaskResult<List<ProfileVm>> unlikedProfiles = await _matchingRepository.GetUnlikedProfiles(loggedInAuthId);
        TaskResult<List<PreferenceVm>> loggedUserPreferences = await _matchingRepository.GetLoggedUserPreferences(loggedInAuthId);
        if (!unlikedProfiles.IsSuccessful)
        {
            return TaskResult<List<UnlikedProfileVm>>.Failure(unlikedProfiles.Message);
        }
        if (!loggedUserPreferences.IsSuccessful)
        {
            return TaskResult<List<UnlikedProfileVm>>.Failure(loggedUserPreferences.Message);
        }
        var unlikedScoredProfiles = new List<UnlikedProfileVm>();
        var userPreferencesList = new List<string>();
        loggedUserPreferences.Result.ForEach(e => userPreferencesList.Add(e.PreferenceTag));
        
        foreach (var profile in unlikedProfiles.Result)
        {
            int matchScore = 0;
            profile.Preferences.ForEach(e =>
                {
                    if (userPreferencesList.Contains(e))
                    {
                        matchScore++;
                    }
                });
            var uProfile = new UnlikedProfileVm
            {
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Email = profile.Email,
                Gender = profile.Gender,
                BirthDate = profile.BirthDate,
                PhoneNumber = profile.PhoneNumber,
                MatchScore = matchScore
            };
            unlikedScoredProfiles.Add(uProfile);
        }
        return TaskResult<List<UnlikedProfileVm>>.Success(unlikedScoredProfiles);
    }

    public async Task<TaskResult<ActionResult>> CreateLike(string loggedInAuthId, Guid likeeUserId)
    {
        TaskResult<ActionResult> result = await _matchingRepository.CreateLike(loggedInAuthId, likeeUserId);
        if (!result.IsSuccessful)
        {
            return TaskResult<ActionResult>.Failure(result.Message);
        }

        return TaskResult<ActionResult>.Success(result.Result);
    }

    public async Task<TaskResult<ActionResult>> DeleteMatch(string loggedInAuthId, Guid likeeUserId)
    {
        TaskResult<ActionResult> result = await _matchingRepository.DeleteMatch(loggedInAuthId, likeeUserId);
        if (!result.IsSuccessful)
        {
            return TaskResult<ActionResult>.Failure(result.Message);
        }

        return TaskResult<ActionResult>.Success(result.Result);
    }

    public async Task<TaskResult<List<ProfileVm>>> GetUserMatchProfiles(string loggedInAuthId)
    {
        TaskResult<List<ProfileVm>> result = await _matchingRepository.GetUserMatchProfiles(loggedInAuthId);
        if (!result.IsSuccessful)
        {
            return TaskResult<List<ProfileVm>>.Failure(result.Message);
        }

        return TaskResult<List<ProfileVm>>.Success(result.Result);
    }

    public async Task<TaskResult<List<PreferenceVm>>> GetLoggedUserPreferences(string loggedInAuthId)
    {
        TaskResult<List<PreferenceVm>> result = await _matchingRepository.GetLoggedUserPreferences(loggedInAuthId);
        if (!result.IsSuccessful)
        {
            return TaskResult<List<PreferenceVm>>.Failure(result.Message);
        }
        return TaskResult<List<PreferenceVm>>.Success(result.Result);
    }

    public async Task<TaskResult<List<PreferenceVm>>> GetPreferences()
    {
        TaskResult<List<PreferenceVm>> result = await _matchingRepository.GetPreferences();
        if (!result.IsSuccessful)
        {
            return TaskResult<List<PreferenceVm>>.Failure(result.Message);
        }

        return TaskResult<List<PreferenceVm>>.Success(result.Result);
    }

    public async Task<TaskResult<ActionResult>> CreatePreferencesUser(PreferenceUserRequest request, string loggedInAuthId)
    {
        TaskResult<ActionResult> result = await _matchingRepository.CreatePreferencesUser(request, loggedInAuthId);
        if (!result.IsSuccessful)
        {
            return TaskResult<ActionResult>.Failure(result.Message);
        }

        return TaskResult<ActionResult>.Success(result.Result);
    }

    public async Task<TaskResult<ActionResult>> UpdateUserSearchLocation(SearchLocationRequest request, string loggedInAuthId)
    {
        TaskResult<ActionResult> result = await _matchingRepository.UpdateUserSearchLocation(request, loggedInAuthId);
        if (!result.IsSuccessful)
        {
            return TaskResult<ActionResult>.Failure(result.Message);
        }

        return TaskResult<ActionResult>.Success(result.Result);
    }

}