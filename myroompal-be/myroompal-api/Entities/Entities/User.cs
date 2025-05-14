using myroompal_api.Entities.Types;

namespace myroompal_api.Entities.Entities;


public class User
{
    public Guid Id { get; set; }
    public required string Auth0Id { get; set; }
    public required UserRoleType RoleName { get; set; }
    
    public required UserAccountStatusType Status { get; set; }
    
    public Profile? Profile { get; set; }
    
    public ICollection<PreferenceUser> PreferencesUsers { get; } = new List<PreferenceUser>();

    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
    
    public ICollection<Room> Rooms { get; } = new List<Room>();
    public ICollection<Rent> Rents { get; } = new List<Rent>();

    public ICollection<Like> Likes { get; } = new List<Like>();
    public ICollection<Match> Matches { get; } = new List<Match>();
    public ICollection<UserRating> Ratings { get; } = new List<UserRating>();
    public ICollection<SupportTicket> SupportTickets { get; } = new List<SupportTicket>();
}