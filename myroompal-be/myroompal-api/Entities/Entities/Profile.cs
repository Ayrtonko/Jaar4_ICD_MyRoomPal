namespace myroompal_api.Entities.Entities;

public class Profile
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateOnly BirthDate { get; set; }
    public required string Gender { get; set;}
    public required string PhoneNumber { get; set; }

    public string? SearchLocation { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; }
}