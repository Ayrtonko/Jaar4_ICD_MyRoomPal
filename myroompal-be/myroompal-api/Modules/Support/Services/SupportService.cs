using myroompal_api.Entities.Entities;
using myroompal_api.Entities.Types;
using myroompal_api.Modules.Shared;
using myroompal_api.Modules.Support.Interfaces;

namespace myroompal_api.Modules.Support.Services;

public class SupportService : ISupportService
{
    private readonly ISupportRepository _supportRepository;
    public SupportService(ISupportRepository supportRepository)
    {
        _supportRepository = supportRepository;
    }

    public async Task<TaskResult<SupportTicket>> CreateSupportTicket(SupportTicket? supportTicket)
    {
        if (supportTicket == null ||
            supportTicket.CreatorOfTicketId == Guid.Empty ||
            string.IsNullOrWhiteSpace(supportTicket.Description))
            return TaskResult<SupportTicket>.Failure("Invalid support ticket data! Could not create support ticket.");
        
        TaskResult<SupportTicket> result = await _supportRepository.CreateSupportTicket(supportTicket);
        
        if (!result.IsSuccessful)
            return TaskResult<SupportTicket>.Failure(result.Message);
        
        
        return TaskResult<SupportTicket>.Success(supportTicket, result.Message);
    }

    public async Task<TaskResult<List<SupportTicket>>> GetAllTickets()
    {
        return await _supportRepository.GetAllTickets();
    }

    public async Task<TaskResult<List<SupportTicket>>> GetTicketsById(List<Guid> supportTicketIds)
    {
        if (supportTicketIds.Count == 0)
            return TaskResult<List<SupportTicket>>.Failure("Invalid support ticket data! Could not fetch support tickets.");
        
        List<TaskResult<SupportTicket>> results = new();
        foreach (Guid supportTicketId in supportTicketIds)
        {
            TaskResult<SupportTicket> result = await _supportRepository.GetTicketById(supportTicketId);
            results.Add(result);
        }

        if (results.Any(r => !r.IsSuccessful))
        {
            return TaskResult<List<SupportTicket>>.Failure("Some support tickets could not be fetched. Please try again.");
        }
        return TaskResult<List<SupportTicket>>.Success(results.Select(r => r.Result).ToList(), "Support tickets fetched successfully.");
    }

    public async Task<TaskResult<List<SupportTicket>>> UpdateSupportTicketsStatus(List<SupportTicket> supportTickets, SupportTicketStatus newStatus)
    {
        try
        {
            if (supportTickets.Count == 0)
                return TaskResult<List<SupportTicket>>.Failure(
                    "Invalid support ticket data! Could not update support ticket status.");
            
            //Change tickets status to new status
            supportTickets.ForEach(s => s.Status = newStatus);

            TaskResult<List<SupportTicket>> result = await _supportRepository.UpdateSupportTicketsStatus(supportTickets);
            if (!result.IsSuccessful)
                return TaskResult<List<SupportTicket>>.Failure(result.Message);
            
            return TaskResult<List<SupportTicket>>.Success(supportTickets,
                "Support ticket status updated successfully.");
        }
        catch (Exception e)
        {
            return TaskResult<List<SupportTicket>>.Failure("Something went wrong! Could not update support ticket status. " + e.Message);
        }
    }
}