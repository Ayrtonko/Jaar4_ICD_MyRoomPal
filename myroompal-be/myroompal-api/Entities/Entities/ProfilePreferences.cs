namespace myroompal_api.Entities.Entities;

public class ProfilePreferences{

    public Guid Id { get; set; }
    public string? SleepingHabits{ get; set; }
    public string? SocialHabits{ get; set; }
    public string? CleaningHabits{ get; set; }
    public string? DietaryPreferences{ get; set; }
    public string? Occupation{ get; set; }
    public string? SchoolOrCompany{ get; set; }
    public string? ReligiousPreferences{ get; set; }
    public bool? SmokingHabits { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
    
}