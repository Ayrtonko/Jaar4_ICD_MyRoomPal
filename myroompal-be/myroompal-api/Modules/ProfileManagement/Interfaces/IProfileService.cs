using myroompal_api.Modules.Shared;
using myroompal_api.Entities.Entities;

namespace myroompal_api.Modules.ProfileManagement.Models;

public interface IProfileService
{
    Task<TaskResult<ProfilePageVm>> GetProfileByAuth0Id(string auth0Id);
    Task<TaskResult<ProfilePreferences>> CreateOrUpdateProfile(ProfilePreferencesVm profilePreferencesVm);
}
