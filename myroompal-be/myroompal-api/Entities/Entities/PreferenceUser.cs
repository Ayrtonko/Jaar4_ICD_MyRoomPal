using myroompal_api.Entities.Types;

namespace myroompal_api.Entities.Entities;

public class PreferenceUser
{
    public Guid Id { get; set; }
    public required string PreferenceTag { get; set; }
    public Guid? PreferenceId { get; set; }
    public Preference? Preference { get; set; }
    public required Guid UserId { get; set; }
    public required User User { get; set; }
}