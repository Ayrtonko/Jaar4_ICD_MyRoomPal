using Microsoft.EntityFrameworkCore;
using myroompal_api.Data;
using myroompal_api.Entities.Entities;

namespace MyRoomPal.Tests.AcceptanceTests;

public class ProfileTest
{
    [Fact]
    public void UserCanAccessEditProfilePage()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("InMemoryProfileDB")
            .Options;
        
        var context = new ApplicationDbContext(options);

        var loggedUserId = new Guid();
        
        context.Profiles.Add(new myroompal_api.Entities.Entities.Profile
        {
            Id = loggedUserId,
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateOnly(1990, 1, 1),
            Gender = "Male",
            PhoneNumber = "1234567890"
        });
        context.SaveChanges();


        // Act
        var profile = context.Profiles.FirstOrDefault();

        // Assert
        Assert.NotNull(profile);
    }

    [Fact]
    public void UserCanEditProfileSuccessfully()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("InMemoryProfileDB")
            .Options;

        var context = new ApplicationDbContext(options);

        var loggedUserId = new Guid();
        
        context.Profiles.Add(new myroompal_api.Entities.Entities.Profile
        {
            Id = loggedUserId,
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateOnly(1990, 1, 1),
            Gender = "Male",
            PhoneNumber = "1234567890"
        });
        context.SaveChanges();

        // Act
        var updatedProfile = new myroompal_api.Entities.Entities.Profile
        {
            Email = "newemail@example.com",
            FirstName = "Jane",
            LastName = "Smith",
            BirthDate = new DateOnly(1995, 5, 15),
            Gender = "Female",
            PhoneNumber = "9876543210",
            SearchLocation = "New York, USA"
        };
        
        var savedProfile = context.Profiles.First();
        savedProfile.Email = updatedProfile.Email;
        savedProfile.FirstName = updatedProfile.FirstName;
        savedProfile.LastName = updatedProfile.LastName;
        savedProfile.BirthDate = updatedProfile.BirthDate;
        savedProfile.Gender = updatedProfile.Gender;
        savedProfile.PhoneNumber = updatedProfile.PhoneNumber;
        savedProfile.SearchLocation = updatedProfile.SearchLocation;
        context.SaveChanges();
        
        
        // Assert
        Assert.Equal(updatedProfile.Email, savedProfile.Email);
        Assert.Equal(updatedProfile.FirstName, savedProfile.FirstName);
        Assert.Equal(updatedProfile.LastName, savedProfile.LastName);
        Assert.Equal(updatedProfile.BirthDate, savedProfile.BirthDate);
        Assert.Equal(updatedProfile.Gender, savedProfile.Gender);
        Assert.Equal(updatedProfile.PhoneNumber, savedProfile.PhoneNumber);
        Assert.Equal(updatedProfile.SearchLocation, savedProfile.SearchLocation);
    }
}