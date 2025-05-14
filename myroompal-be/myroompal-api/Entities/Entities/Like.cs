namespace myroompal_api.Entities.Entities;

public class Like
{
    public Guid Id { get; set; } 
    
    public DateOnly LikeDate { get; set; }
    
    public Guid LikerUserId { get; set; }
    public required User LikerUser { get; set; }

    public required Guid LikeeUserId { get; set; }
    
}