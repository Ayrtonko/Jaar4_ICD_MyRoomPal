using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.Shared;
using myroompal_api.Modules.UserManagement.Interfaces;
using myroompal_api.Modules.UserManagement.Models;
using myroompal_api.Modules.UserManagement.Repositories;

namespace myroompal_api.Modules.UserManagement.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<TaskResult<List<User>>> GetUsers()
    {
        var users = await _userRepository.GetUsers();
        if (!users.IsSuccessful)
            return TaskResult<List<User>>.Failure("Failed to get users");
        
        return TaskResult<List<User>>.Success(users.Result);
    }

    public async Task<TaskResult<Guid>> GetUserById(string auth0Id)
    {
        if (string.IsNullOrEmpty(auth0Id))
            return TaskResult<Guid>.Failure("Invalid user id");
        
        var user = await _userRepository.GetUserById(auth0Id);
        if (!user.IsSuccessful)
            return TaskResult<Guid>.Failure("Failed to get user");
        
        return TaskResult<Guid>.Success(user.Result);
    }
    public async Task<TaskResult<User>> CreateUser(UserVm userVm)
    {
        if(userVm == null)
            return TaskResult<User>.Failure("User cannot be null");

        var createUser = await _userRepository.CreateUser(userVm);
        return TaskResult<User>.Success(createUser.Result);
    }

    public async Task<TaskResult<List<UserSearchVm>>> GetUserBySearchQuery(string searchQuery)
    {
        if (string.IsNullOrEmpty(searchQuery))
            return TaskResult<List<UserSearchVm>>.Failure(
                "Search query cannot be empty");
        
        var matchedUsers = await _userRepository.GetUserBySearchQuery(searchQuery);
        if (!matchedUsers.IsSuccessful)
            return TaskResult<List<UserSearchVm>>.Failure("Failed to get users");
        
        return TaskResult<List<UserSearchVm>>.Success(matchedUsers.Result);
    }

    public async Task<TaskResult<UserSearchVm>> UpdateUserStatus(UserSearchVm userSearchVm)
    {
        if (userSearchVm.Id == Guid.Empty ||
            userSearchVm.Id == null ||
            userSearchVm.Status == null)
            return TaskResult<UserSearchVm>.Failure("Invalid user id or status");
        
        var updatedUser = await _userRepository.UpdateUserStatus(userSearchVm);
        if (!updatedUser.IsSuccessful)
            return TaskResult<UserSearchVm>.Failure("Failed to update user status");
        
        return TaskResult<UserSearchVm>.Success(updatedUser.Result);
    }

    public async Task<TaskResult<UserRoleDto>> GetUserRole(string auth0Id)
    {
        var userRole = await _userRepository.GetUserRole(auth0Id);
        if (!userRole.IsSuccessful)
            return TaskResult<UserRoleDto>.Failure("Failed to get user role");
        
        return TaskResult<UserRoleDto>.Success(userRole.Result);
    }
}