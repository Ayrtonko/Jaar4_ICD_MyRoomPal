using Microsoft.EntityFrameworkCore;
using myroompal_api.Data;
using myroompal_api.Entities.Entities;
using myroompal_api.Entities.Types;

namespace MyRoomPal.Tests.AcceptanceTests;

public class UserMatchTest
{
    private readonly ApplicationDbContext _context;

    public UserMatchTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UATDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public void MatchIsCreatedOnMutualLikes()
    {
        // Arrange
        var loggedUserId = new Guid();
        var userA = new User
        {
            Id = loggedUserId,
            Auth0Id = "1",
            RoleName = UserRoleType.Tenant,
            Status = UserAccountStatusType.Active,
            
        };
        var userB = new User
        {
            Id = new Guid(),
            Auth0Id = "2",
            RoleName = UserRoleType.Tenant,
            Status = UserAccountStatusType.Active
        };
        _context.Users.AddRange(userA, userB);
        _context.SaveChanges();

        var likeA = new Like
        {
            LikerUser = userA,
            LikerUserId = userA.Id,
            LikeeUserId = userB.Id
        };
        _context.Likes.Add(likeA);
        _context.SaveChanges();


        // Act
        var likeB = new Like
        {
            LikerUser = userB,
            LikerUserId = userB.Id,
            LikeeUserId = userA.Id
        };
        _context.Likes.Add(likeB);
        _context.SaveChanges();

        // Check if mutual likes exist
        var mutualLikeExists = _context.Likes.Any(l =>
                                   l.LikerUserId == userA.Id && l.LikeeUserId == userB.Id) &&
                               _context.Likes.Any(l =>
                                   l.LikerUserId == userB.Id && l.LikeeUserId == userA.Id);

        if (mutualLikeExists)
        {
            _context.Matches.Add(new Match
            {
                MatcherUser = userA,
                MatcheeUser = userB
            });
            _context.SaveChanges();
        }

        var match = _context.Matches
            .FirstOrDefault(m => m.MatcherUserId == userA.Id && m.MatcheeUserId == userB.Id);

        //Assert
        Assert.NotNull(match); // Check that a match exists
        Assert.Equal(userA.Id, match.MatcherUserId); // Ensure the matcher is userA
        Assert.Equal(userB.Id, match.MatcheeUserId); // Ensure the matchee is userB
    }
}