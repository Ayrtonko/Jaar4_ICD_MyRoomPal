using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.Support.Models;
using myroompal_api.Modules.Shared;
using myroompal_api.Modules.Support.Interfaces;
using myroompal_api.Modules.UserManagement.Interfaces;

namespace myroompal_api.Modules.Support.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupportController : ControllerBase
{
    
    private readonly ISupportService _supportService;
    private readonly IUserService _userService;
    private readonly IAuth0Context _auth0Context;
    private readonly ILogger<SupportController> _logger;

    public SupportController(ISupportService supportService, IUserService userService,IAuth0Context auth0Context, ILogger<SupportController> logger)
    {
        _supportService = supportService;
        _userService = userService;
        _auth0Context = auth0Context;
        _logger = logger;
    }
    
    // /api/support
    // Create a new support ticket
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<SupportTicket>> CreateSupportTicket([FromBody] SupportTicketVm? supportTicketVm)
    {
        if (supportTicketVm == null ||
            string.IsNullOrWhiteSpace(supportTicketVm.Description))
            return BadRequest(TaskResult<SupportTicketVm>.Failure("Invalid support ticket data! Could not create support ticket."));

        try
        {
            var auth0Id = _auth0Context.GetAuth0Id(User);
            var user = await _userService.GetUserById(auth0Id);
            if (!user.IsSuccessful)
                return BadRequest(TaskResult<SupportTicketVm>.Failure("Invalid user data! Could not create support ticket."));

            supportTicketVm.CreatorOfTicketId = user.Result;
            TaskResult<SupportTicket> result = await _supportService.CreateSupportTicket(supportTicketVm.ToEntity());

            if (!result.IsSuccessful)
            {
                return BadRequest(TaskResult<SupportTicketVm>.Failure(result.Message));
            }

            return Ok("Support ticket created successfully.");
        }
        catch (Exception e)
        {
            return BadRequest(TaskResult<SupportTicketVm>.Failure("Something went wrong! Could not create support ticket. " + e.Message));
        }
    }
    
    // /api/support
    // Fetches all support tickets
    // Must have admin role
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<SupportTicket>> GetAllTickets()
    {
        _logger.LogInformation("GetAllTickets method called.");
        try
        {
            var isAdmin = _auth0Context.IsAdmin(User);
            if (!isAdmin)
                return Unauthorized("You are not authorized to access this resource.");

            TaskResult<List<SupportTicket>> result = await _supportService.GetAllTickets();

            if (!result.IsSuccessful)
            {
                return BadRequest(TaskResult<List<SupportTicket>>.Failure(result.Message));
            } 
            return Ok(result.Result.Select(SupportTicketVm.CreateFrom).ToList());
            
        }
        catch (Exception e)
        {
            return BadRequest(TaskResult<List<SupportTicket>>.Failure("Something went wrong! Could not get all support tickets. " + e.Message));
        }
    }
    
    // /api/support
    // Updates the status of multiple support tickets 
    // Must have admin role
    [HttpPut]
    [Authorize]
    public async Task<ActionResult<List<SupportTicket>>> UpdateSupportTicketStatus(SupportTicketPatchVm supportTicketPatchVms)
    {
        try
        {
            if (supportTicketPatchVms.SupportTicketIds.Count == 0)
                return BadRequest("Invalid support ticket data! Could not update support ticket status.");

            
            //Fetch existing support tickets
            TaskResult<List<SupportTicket>> existingSupportTicketsResult = await _supportService.GetTicketsById(supportTicketPatchVms.SupportTicketIds);
            if (!existingSupportTicketsResult.IsSuccessful)
            {
                return BadRequest(existingSupportTicketsResult.Message);
            }
            
            TaskResult<List<SupportTicket>> result =
                await _supportService.UpdateSupportTicketsStatus(existingSupportTicketsResult.Result, supportTicketPatchVms.Status);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.Message);
            }

            return Ok(SupportTicketPatchVm.CreateFrom(result.Result));
        }
        catch (Exception e)
        {
            return BadRequest("Something went wrong! Could not update support ticket status. " + e.Message);
        }
    }
}