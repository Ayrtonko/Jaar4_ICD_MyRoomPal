using Microsoft.EntityFrameworkCore;
using myroompal_api.Data;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.ProfileManagement.Models;
using myroompal_api.Modules.Shared;

public class ProfileRepository : IProfileRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context; // Database context
    }

public async Task<TaskResult<ProfilePreferences>> CreateOrUpdateProfile(ProfilePreferences profilePreferences)
{
    try
    {
        Console.WriteLine($"UserId: {profilePreferences.UserId}");
        var existingProfile = await _context.ProfilePreferences
            .FirstOrDefaultAsync(p => p.UserId == profilePreferences.UserId);

        if (existingProfile == null) 
        {
            profilePreferences.Id = Guid.NewGuid();
            await _context.ProfilePreferences.AddAsync(profilePreferences);
        }
        else 
        {
            existingProfile.SleepingHabits = profilePreferences.SleepingHabits;
            existingProfile.SocialHabits = profilePreferences.SocialHabits;
            existingProfile.CleaningHabits = profilePreferences.CleaningHabits;
            existingProfile.DietaryPreferences = profilePreferences.DietaryPreferences;
            existingProfile.Occupation = profilePreferences.Occupation;
            existingProfile.SchoolOrCompany = profilePreferences.SchoolOrCompany;
            existingProfile.ReligiousPreferences = profilePreferences.ReligiousPreferences;
            existingProfile.SmokingHabits = profilePreferences.SmokingHabits;
        }

        await _context.SaveChangesAsync();
        return TaskResult<ProfilePreferences>.Success(profilePreferences, "Profile updated successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in CreateOrUpdateProfile Repository: {ex.Message}");
        return TaskResult<ProfilePreferences>.Failure($"Error updating profile: {ex.Message}");
    }
}


   public async Task<TaskResult<ProfilePageVm>> GetProfileByAuth0Id(string auth0Id)
{
    var profile = await _context.Profiles
        .Include(p => p.User)
        .Include(p => p.User.Address)
        .FirstOrDefaultAsync(p => p.User.Auth0Id == auth0Id);

    if (profile == null)
    {
        return TaskResult<ProfilePageVm>.Failure("Profile not found.");
    }
    var preferences = await _context.ProfilePreferences
        .FirstOrDefaultAsync(p => p.UserId == profile.UserId);

    var entity = new ProfilePageVm
    {
        Name = char.ToUpper(profile.FirstName[0]) + profile.FirstName.Substring(1).ToLower() + " " +
               char.ToUpper(profile.LastName[0]) + profile.LastName.Substring(1).ToLower(),
        Age = DateTime.Today.Year - profile.BirthDate.Year
                                  - (DateOnly.FromDateTime(DateTime.Today) < profile.BirthDate ? 1 : 0),
        Email = profile.Email,
        PhoneNumber = profile.PhoneNumber,
        Location = profile.User.Address.City + ", " + profile.User.Address.Country,
        ProfilePreferences = new ProfilePreferencesVm {
            SleepingHabits = preferences?.SleepingHabits ?? null,
            SocialHabits = preferences?.SocialHabits ?? null,
            CleaningHabits = preferences?.CleaningHabits ?? null,
            DietaryPreferences = preferences?.DietaryPreferences ?? null,
            Occupation = preferences?.Occupation ?? null,
            SchoolOrCompany = preferences?.SchoolOrCompany ?? null,
            ReligiousPreferences = preferences?.ReligiousPreferences ?? null,
            SmokingHabits = preferences?.SmokingHabits ?? null,
            UserId = profile.User.Id
        }
    };
    return TaskResult<ProfilePageVm>.Success(entity);
}

}
