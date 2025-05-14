using Microsoft.EntityFrameworkCore;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.Shared;
using myroompal_api.Modules.Support.Interfaces;
using myroompal_api.Data;
using myroompal_api.Entities.Types;

namespace myroompal_api.Modules.Support.Repositories;

public class SupportRepository : ISupportRepository
{
    private readonly ApplicationDbContext _context;

    public SupportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskResult<SupportTicket>> CreateSupportTicket(SupportTicket supportTicket)
    {
        try
        {
            supportTicket.Id = Guid.NewGuid();

            await _context.SupportTickets.AddAsync(supportTicket);
            await _context.SaveChangesAsync();
            return TaskResult<SupportTicket>.Success(supportTicket, "Support ticket created successfully.");
        }
        catch (Exception ex)
        {
            return TaskResult<SupportTicket>.Failure($"Error creating support ticket: {ex.Message}");
        }
    }

    public Task<TaskResult<List<SupportTicket>>> GetAllTickets()
    {
        try
        {
            List<SupportTicket> supportTickets = _context.SupportTickets.ToList();
            return Task.FromResult(TaskResult<List<SupportTicket>>.Success(supportTickets, "Support tickets retrieved successfully."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(TaskResult<List<SupportTicket>>.Failure($"Error retrieving support tickets: {ex.Message}"));
        }
    }

    public Task<TaskResult<SupportTicket>> GetTicketById(Guid supportTicketId)
    {
        if (supportTicketId == Guid.Empty)
            return Task.FromResult(TaskResult<SupportTicket>.Failure("Invalid support ticket id."));
        try
        {
            SupportTicket? supportTicket = _context.SupportTickets.FirstOrDefault(s => s.Id == supportTicketId);
            if (supportTicket == null)
                return Task.FromResult(TaskResult<SupportTicket>.Failure("Support ticket not found."));

            return Task.FromResult(TaskResult<SupportTicket>.Success(supportTicket, "Support ticket retrieved successfully."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(TaskResult<SupportTicket>.Failure($"Error retrieving support ticket: {ex.Message}"));
        }
    }

    public async Task<TaskResult<List<SupportTicket>>> UpdateSupportTicketsStatus(List<SupportTicket> supportTickets)
    {
        try
        {
            _context.SupportTickets.UpdateRange(supportTickets);
            await _context.SaveChangesAsync();
            return TaskResult<List<SupportTicket>>.Success(supportTickets, "Support ticket status updated successfully.");
        }
        catch (Exception ex)
        {
            return TaskResult<List<SupportTicket>>.Failure($"Error updating support ticket status: {ex.Message}");
        }
    }
}