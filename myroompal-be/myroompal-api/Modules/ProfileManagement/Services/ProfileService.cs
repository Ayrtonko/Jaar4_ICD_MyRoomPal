using myroompal_api.Entities.Entities;
using myroompal_api.Modules.ProfileManagement.Models;
using myroompal_api.Modules.Shared;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;

    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

public async Task<TaskResult<ProfilePreferences>> CreateOrUpdateProfile(ProfilePreferencesVm profilePreferencesVm)
{
    try
    {
        if (profilePreferencesVm == null)
        {
            return TaskResult<ProfilePreferences>.Failure("Profile data is null.");
        }

        var profileEntity = new ProfilePreferences
        {
            UserId = profilePreferencesVm.UserId,
            SleepingHabits = profilePreferencesVm.SleepingHabits,
            SocialHabits = profilePreferencesVm.SocialHabits,
            CleaningHabits = profilePreferencesVm.CleaningHabits,
            DietaryPreferences = profilePreferencesVm.DietaryPreferences,
            Occupation = profilePreferencesVm.Occupation,
            SchoolOrCompany = profilePreferencesVm.SchoolOrCompany,
            ReligiousPreferences = profilePreferencesVm.ReligiousPreferences,
            SmokingHabits = profilePreferencesVm.SmokingHabits
        };

        return await _profileRepository.CreateOrUpdateProfile(profileEntity);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in CreateOrUpdateProfile Service: {ex.Message}");
        return TaskResult<ProfilePreferences>.Failure("An error occurred while updating the profile.");
    }
}


public async Task<TaskResult<ProfilePageVm>> GetProfileByAuth0Id(string auth0Id)
{
    try
    {
        var profileVm = await _profileRepository.GetProfileByAuth0Id(auth0Id);
        if (!profileVm.IsSuccessful)
        {
            return TaskResult<ProfilePageVm>.Failure("Profile not found.");
        }

        return TaskResult<ProfilePageVm>.Success(profileVm.Result, "Profile retrieved successfully.");
    }
    catch (Exception ex)
    {
        return TaskResult<ProfilePageVm>.Failure($"Error retrieving profile: {ex.Message}");
    }
}

    
}
