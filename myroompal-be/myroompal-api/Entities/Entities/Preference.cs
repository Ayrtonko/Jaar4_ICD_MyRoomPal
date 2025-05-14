using myroompal_api.Entities.Types;

namespace myroompal_api.Entities.Entities;

public class Preference
{
    public Guid Id { get; set; }
    public required string PreferenceTag { get; set; }

    public ICollection<PreferenceUser> PreferencesUsers { get; } = new List<PreferenceUser>();
}