using myroompal_api.Entities.Entities;
using myroompal_api.Entities.Types;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.Support.Interfaces;

public interface ISupportService
{ 
    Task<TaskResult<SupportTicket>> CreateSupportTicket(SupportTicket? supportTicket);
    Task<TaskResult<List<SupportTicket>>> GetAllTickets();
    Task<TaskResult<List<SupportTicket>>> GetTicketsById(List<Guid> supportTicketIds);
    Task<TaskResult<List<SupportTicket>>> UpdateSupportTicketsStatus(List<SupportTicket> supportTickets, SupportTicketStatus newStatus);
}