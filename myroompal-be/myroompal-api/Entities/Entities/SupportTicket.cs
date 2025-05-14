using myroompal_api.Entities.Types;

namespace myroompal_api.Entities.Entities;

public class SupportTicket
{
    public Guid Id { get; set; }
    public SupportTicketIssueType IssueType { get; set; }
    public string Description { get; set; } = String.Empty;
    public required SupportTicketStatus Status { get; set; }
    public Guid CreatorOfTicketId { get; set; }
    public User? CreatorOfTicket { get; set; }

    // Parameterless constructor required by EF Core
    public SupportTicket() { }

    // Constructor for creating instances
    public SupportTicket(SupportTicketIssueType issueType, string description, SupportTicketStatus status, Guid creatorOfTicketId)
    {
        IssueType = issueType;
        Description = description;
        Status = status;
        CreatorOfTicketId = creatorOfTicketId;
    }
}