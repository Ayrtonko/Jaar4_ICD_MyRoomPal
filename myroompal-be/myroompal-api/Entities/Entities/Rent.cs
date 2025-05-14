namespace myroompal_api.Entities.Entities;

public class Rent
{
    public Guid Id { get; set; }
    public required DateTime DueDate { get; set; }

    public Guid? RoomId { get; set; }
    public Room? Room { get; set; } 

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public ICollection<Payment> Payments { get; } = new List<Payment>();
}