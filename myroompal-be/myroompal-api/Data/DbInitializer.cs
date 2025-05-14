using myroompal_api.Entities.Entities;
using myroompal_api.Entities.Types;

namespace myroompal_api.Data;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context, bool isProduction)
    {
        SynchronizePreferences(context);
        context.SaveChanges();

        if (isProduction)
        {
            return;
        }

        SeedUsers(context);
        context.SaveChanges();

        SeedProfiles(context);
        context.SaveChanges();
    }

    private static void SynchronizePreferences(ApplicationDbContext context)
    {
        var predefinedPreferences = new List<Preference>
        {
            new Preference { PreferenceTag = "Near public transport" },
            new Preference { PreferenceTag = "Pet-friendly" },
            new Preference { PreferenceTag = "Includes parking" },
            new Preference { PreferenceTag = "A backyard" },
            new Preference { PreferenceTag = "Separates waste" },
            new Preference { PreferenceTag = "Energy-efficient appliances" },
            new Preference { PreferenceTag = "Enjoys cooking with others" },
            new Preference { PreferenceTag = "Wheelchair accessible" },
            new Preference { PreferenceTag = "Enjoys social gatherings" },
            new Preference { PreferenceTag = "Near shopping centers" },
            new Preference { PreferenceTag = "High-speed internet availability" },
            new Preference { PreferenceTag = "Modern kitchen" }
        };
        var existingPreferences = context.Preferences.ToList();

        // Add new preferences
        foreach (var preference in predefinedPreferences)
        {
            if (existingPreferences.All(p => p.PreferenceTag != preference.PreferenceTag))
            {
                context.Preferences.Add(preference);
            }
        }

        // Remove old preferences
        foreach (var preference in existingPreferences)
        {
            if (predefinedPreferences.All(p => p.PreferenceTag != preference.PreferenceTag))
            {
                context.Preferences.Remove(preference);
            }
        }
    }

    private static void SeedUsers(ApplicationDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    Auth0Id = "1",
                    RoleName = UserRoleType.Tenant,
                    Status = UserAccountStatusType.Active
                },
                new User
                {
                    Auth0Id = "2",
                    RoleName = UserRoleType.Tenant,
                    Status = UserAccountStatusType.Active
                },
                new User
                {
                    Auth0Id = "3",
                    RoleName = UserRoleType.Tenant,
                    Status = UserAccountStatusType.Active
                },
                new User
                {
                    Auth0Id = "4",
                    RoleName = UserRoleType.Tenant,
                    Status = UserAccountStatusType.Active
                },
                new User
                {
                    Auth0Id = "5",
                    RoleName = UserRoleType.Tenant,
                    Status = UserAccountStatusType.Active
                }
            );
        }
    }

    private static void SeedProfiles(ApplicationDbContext context)
    {
        if (!context.Profiles.Any())
        {
            var users = context.Users.Take(5).ToList();

            if (users.Count >= 5)
            {
                context.Profiles.AddRange(
                    new Profile
                    {
                        Email = "p0@test.nl",
                        FirstName = "Abraham",
                        LastName = "Lincoln",
                        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)), // Age: 20
                        Gender = "Male",
                        PhoneNumber = "0620194765",
                        User = users[0],
                    },
                    new Profile
                    {
                        Email = "p1@test.nl",
                        FirstName = "Marie",
                        LastName = "Curie",
                        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-25)), // Age: 25
                        Gender = "Female",
                        PhoneNumber = "0620194766",
                        User = users[1],
                    },
                    new Profile
                    {
                        Email = "p2@test.nl",
                        FirstName = "Isaac",
                        LastName = "Newton",
                        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-18)), // Age: 18
                        Gender = "Male",
                        PhoneNumber = "0620194767",
                        User = users[2],
                    },
                    new Profile
                    {
                        Email = "p3@test.nl",
                        FirstName = "Ada",
                        LastName = "Lovelace",
                        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-30)), // Age: 30
                        Gender = "Female",
                        PhoneNumber = "0620194768",
                        User = users[3],
                    },
                    new Profile
                    {
                        Email = "p4@test.nl",
                        FirstName = "Albert",
                        LastName = "Einstein",
                        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-32)), // Age: 32
                        Gender = "Male",
                        PhoneNumber = "0620194769",
                        User = users[4],
                    }
                );
            }
        }
    }
}