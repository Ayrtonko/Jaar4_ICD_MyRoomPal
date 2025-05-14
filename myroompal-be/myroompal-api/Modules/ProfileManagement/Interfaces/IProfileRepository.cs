using myroompal_api.Modules.Shared;
using myroompal_api.Entities.Entities;

namespace myroompal_api.Modules.ProfileManagement.Models;

public interface IProfileRepository
{
    Task<TaskResult<ProfilePreferences>> CreateOrUpdateProfile(ProfilePreferences profilePreferences);
    Task<TaskResult<ProfilePageVm>>  GetProfileByAuth0Id(string auth0Id);
}
