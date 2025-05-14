using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using myroompal_api.Modules.UserManagement.Interfaces;

namespace MyRoomPal.Tests;
using Moq; 
using Microsoft.AspNetCore.Mvc; 
using myroompal_api.Modules.Support.Controllers; 
using myroompal_api.Modules.Support.Interfaces; 
using myroompal_api.Modules.Support.Models;
using Microsoft.Extensions.Logging; 
using myroompal_api.Modules.Shared; 
using myroompal_api.Entities.Entities;
using myroompal_api.Entities.Types;


public class UnitTest1
{


[Fact]
public async Task CreateSupportTicket_ShouldReturnBadRequest_WhenSupportTicketVmIsInvalid()
{
   // Arrange
    var mockService = new Mock<ISupportService>();
    var mockUserService = new Mock<IUserService>();
    var mockAuth0Context = new Mock<IAuth0Context>();
    var mockLogger = new Mock<ILogger<SupportController>>();

    var creatorOfTicket = Guid.NewGuid();
    var userId = "auth0|123456";

    // Mock input SupportTicketVm
    var supportTicketVm = new SupportTicketVm
    {
        CreatorOfTicketId = creatorOfTicket,
        Description = "Test ticket",
    };

    // Mock ClaimsPrincipal and Auth0 context
    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }, "mock"));

    mockAuth0Context
        .Setup(context => context.GetAuth0Id(claimsPrincipal))
        .Returns(userId);

    // Mock user retrieval
    mockUserService
        .Setup(service => service.GetUserById(userId))
        .ReturnsAsync(TaskResult<Guid>.Success(creatorOfTicket));

    // Mock support ticket creation
    mockService
        .Setup(service => service.CreateSupportTicket(It.IsAny<SupportTicket>()))
        .ReturnsAsync(TaskResult<SupportTicket>.Success(new SupportTicket
        {
            Id = Guid.NewGuid(),
            IssueType = SupportTicketIssueType.Other,
            Description = "Test ticket",
            Status = SupportTicketStatus.New,
            CreatorOfTicketId = creatorOfTicket
        }));

    var controller = new SupportController(mockService.Object, mockUserService.Object, mockAuth0Context.Object, mockLogger.Object)
    {
        ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        }
    };

    // Act
    var result = await controller.CreateSupportTicket(supportTicketVm);

    // Assert
    Assert.IsType<OkObjectResult>(result.Result);

    // Verify interactions
    mockAuth0Context.Verify(context => context.GetAuth0Id(It.IsAny<ClaimsPrincipal>()), Times.Once);
    mockUserService.Verify(service => service.GetUserById(userId), Times.Once);
    mockService.Verify(service => service.CreateSupportTicket(It.IsAny<SupportTicket>()), Times.Once);
}

[Fact]
public async Task CreateSupportTicket_ShouldReturnOk_WhenSupportTicketIsCreated()
{
    // Arrange
    var mockService = new Mock<ISupportService>();
    var mockUserService = new Mock<IUserService>();
    var mockAuth0Context = new Mock<IAuth0Context>();
    var mockLogger = new Mock<ILogger<SupportController>>();

    var creatorOfTicket = Guid.NewGuid();
    var userId = "auth0|123456";

    // Mock input SupportTicketVm
    var supportTicketVm = new SupportTicketVm
    {
        CreatorOfTicketId = creatorOfTicket,
        Description = "Test ticket",
        IssueType = SupportTicketIssueType.Other,
    };

    // Mock ClaimsPrincipal and Auth0 context
    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }, "mock"));

    mockAuth0Context
        .Setup(context => context.GetAuth0Id(claimsPrincipal))
        .Returns(userId);

    // Mock user retrieval
    mockUserService
        .Setup(service => service.GetUserById(userId))
        .ReturnsAsync(TaskResult<Guid>.Success(creatorOfTicket));

    // Mock support ticket creation
    mockService
        .Setup(service => service.CreateSupportTicket(It.IsAny<SupportTicket>()))
        .ReturnsAsync(TaskResult<SupportTicket>.Success(new SupportTicket
        {
            Id = Guid.NewGuid(),
            IssueType = SupportTicketIssueType.Other,
            Description = "Test ticket",
            Status = SupportTicketStatus.New,
            CreatorOfTicketId = creatorOfTicket
        }));

    var controller = new SupportController(mockService.Object, mockUserService.Object, mockAuth0Context.Object, mockLogger.Object)
    {
        ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        }
    };

    // Act
    var result = await controller.CreateSupportTicket(supportTicketVm);

    // Assert
    Assert.IsType<OkObjectResult>(result.Result);

    // Verify interactions
    mockAuth0Context.Verify(context => context.GetAuth0Id(It.IsAny<ClaimsPrincipal>()), Times.Once);
    mockUserService.Verify(service => service.GetUserById(userId), Times.Once);
    mockService.Verify(service => service.CreateSupportTicket(It.IsAny<SupportTicket>()), Times.Once);
}
}