using System.Text.Json.Serialization;
using myroompal_api.Entities.Types;
using myroompal_api.Entities.Entities;

namespace myroompal_api.Modules.Support.Models;

public class SupportTicketVm
{
    public Guid? Id { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SupportTicketIssueType IssueType { get; set; }
    
    public string Description { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SupportTicketStatus? Status { get; set; } = SupportTicketStatus.New;
    
    public Guid? CreatorOfTicketId { get; set; }

    public SupportTicket ToEntity()
    {
        return new SupportTicket {
            Id = Id ?? Guid.Empty,
            IssueType = IssueType,
            Description = Description,
            Status = Status ?? SupportTicketStatus.New,
            CreatorOfTicketId = CreatorOfTicketId ?? Guid.Empty,
        };
    }

    public static SupportTicketVm CreateFrom(SupportTicket supportTicket)
    {
        return new SupportTicketVm
        {
            Id = supportTicket.Id,
            IssueType = supportTicket.IssueType,
            Description = supportTicket.Description,
            Status = supportTicket.Status,
            CreatorOfTicketId = supportTicket.CreatorOfTicketId
        };
    }
}