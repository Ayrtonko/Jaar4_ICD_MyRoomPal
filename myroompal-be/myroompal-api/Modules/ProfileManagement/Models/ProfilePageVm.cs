namespace myroompal_api.Modules.ProfileManagement.Models;

public class ProfilePageVm
{
    public String Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Location { get; set; }
    public ProfilePreferencesVm ProfilePreferences { get; set; }
}
