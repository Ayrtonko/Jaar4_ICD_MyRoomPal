using myroompal_api.Entities.Entities;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.Support.Interfaces;

public interface ISupportRepository
{ 
    Task<TaskResult<SupportTicket>> CreateSupportTicket(SupportTicket supportTicket);
    Task<TaskResult<List<SupportTicket>>> GetAllTickets();
    Task<TaskResult<SupportTicket>> GetTicketById(Guid supportTicketId);
    Task<TaskResult<List<SupportTicket>>> UpdateSupportTicketsStatus(List<SupportTicket> supportTickets);
}