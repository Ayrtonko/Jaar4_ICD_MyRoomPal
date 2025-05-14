namespace myroompal_api.Entities.Entities;

public class UserRating
{
    public Guid Id { get; set; }
    public required int Rating { get; set; }
    
    public Guid UserId { get; set; }
    public required User User { get; set; }

    public Guid RatedUserId { get; set; }
    public required User RatedUser { get; set; }
}