namespace myroompal_api.Modules.MatchManagement.Models;

public class ProfileVm
{
    public required Guid UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Gender { get; set; }
    public required DateOnly BirthDate { get; set; }
    public required string PhoneNumber { get; set; }
    public string? SearchLocation { get; set; }
    public List<string>? Preferences { get; set; }
}