using System.Text.Json.Serialization;
using myroompal_api.Entities.Entities;
using myroompal_api.Entities.Types;


namespace myroompal_api.Modules.Support.Models;

public class SupportTicketPatchVm
{
    public List<Guid> SupportTicketIds { get; set; } = [];
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SupportTicketStatus Status { get; set; }
    
    public static SupportTicketPatchVm CreateFrom(List<SupportTicket> newSupportTickets)
    {
        return new SupportTicketPatchVm
        {
            SupportTicketIds = newSupportTickets.Select(x => x.Id).ToList(),
            Status = newSupportTickets.First().Status
        };
    }
}