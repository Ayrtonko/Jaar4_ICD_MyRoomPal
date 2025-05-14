using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myroompal_api.Data;
using myroompal_api.Entities.Entities;
using myroompal_api.Entities.Types;
using myroompal_api.Modules.Shared;
using myroompal_api.Modules.UserManagement.Interfaces;
using myroompal_api.Modules.UserManagement.Models;

namespace myroompal_api.Modules.UserManagement.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskResult<List<User>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return TaskResult<List<User>>.Success(users);
    }

    public async Task<TaskResult<Guid>> GetUserById(string auth0Id)
    {
        if(string.IsNullOrEmpty(auth0Id))
            return TaskResult<Guid>.Failure("Auth0Id is null");

        var user = await _context.Users
                                 .Where(i => i.Auth0Id == auth0Id)
                                 .Select(x => x.Id)
                                 .SingleOrDefaultAsync();
        if(user == Guid.Empty)
            return TaskResult<Guid>.Failure("User not found");

        return TaskResult<Guid>.Success(user);
    }

    public async Task<TaskResult<User>> CreateUser(UserVm userVm)
    {
        if(userVm == null)
        {
            return TaskResult<User>.Failure("User cannot be null");
        }

        try
        {
            var address = new Address
            {
                Id = Guid.NewGuid(),
                City = userVm.City,
                Country = userVm.Country,
                PostalCode = userVm.PostalCode,
                StreetName = userVm.StreetName,
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Auth0Id = userVm.Auth0Id,
                RoleName = UserRoleType.Tenant,
                Status = UserAccountStatusType.Active,
                AddressId = address.Id
            };

            var createdProfile = new Profile
            {
                Id = Guid.NewGuid(),
                Email = userVm.Email,
                FirstName = userVm.FirstName,
                LastName = userVm.LastName,
                BirthDate = DateOnly.ParseExact(userVm.DateOfBirth, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                Gender = userVm.Gender,
                PhoneNumber = userVm.PhoneNumber,
                UserId = user.Id
            };

            await _context.Addresses.AddAsync(address);
            await _context.Users.AddAsync(user);
            await _context.Profiles.AddAsync(createdProfile);
            await _context.SaveChangesAsync();

            return TaskResult<User>.Success(user);
        }
        catch (Exception ex)
        {
            return TaskResult<User>.Failure(ex.Message);
        }
    }


    public async Task<TaskResult<List<UserSearchVm>>> GetUserBySearchQuery(string searchQuery)
    {
        var profiles = await _context.Profiles
            .Include(x => x.User)
            .Select(x => x).Where(x =>
            x.FirstName.Contains(searchQuery) ||
            x.LastName.Contains(searchQuery) ||
            x.Email.Contains(searchQuery) ||
            x.PhoneNumber.Contains(searchQuery)
        ).ToListAsync();

        if (profiles.Count == 0)
            return TaskResult<List<UserSearchVm>>.Failure("No users found");

        var searchVms = profiles.Select(x => new UserSearchVm
        {
            Id = x.UserId,
            Name = x.FirstName + " " + x.LastName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            Status = x.User?.Status
        }).ToList();

        return TaskResult<List<UserSearchVm>>.Success(searchVms);
    }

    public Task<TaskResult<UserSearchVm>> UpdateUserStatus(UserSearchVm userSearchVm)
    {
        if (userSearchVm.Status == null)
            return Task.FromResult(TaskResult<UserSearchVm>.Failure("Status is required"));
        
        var user = _context.Users.FirstOrDefault(x => x.Id == userSearchVm.Id);
        if (user == null)
            return Task.FromResult(TaskResult<UserSearchVm>.Failure("User not found"));
        
        user.Status = userSearchVm.Status.Value;
        _context.Users.Update(user);
        _context.SaveChanges();
        
        return Task.FromResult(TaskResult<UserSearchVm>.Success(userSearchVm));
    }

    public Task<TaskResult<UserRoleDto>> GetUserRole(string auth0Id)
    {
        var user = _context.Users.FirstOrDefault(x => x.Auth0Id == auth0Id);
        if (user == null)
            return Task.FromResult(TaskResult<UserRoleDto>.Failure("User not found"));
        
        var role = user.RoleName;

        return Task.FromResult(TaskResult<UserRoleDto>.Success(new UserRoleDto
        {
            RoleName = role
        }));
    }
    
}