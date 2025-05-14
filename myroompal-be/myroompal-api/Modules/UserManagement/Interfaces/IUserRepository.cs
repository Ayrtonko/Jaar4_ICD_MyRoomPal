using System.Security.Claims;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.Shared;
using myroompal_api.Modules.UserManagement.Models;

namespace myroompal_api.Modules.UserManagement.Interfaces;

public interface IUserRepository
{
    Task<TaskResult<List<User>>> GetUsers();
    Task<TaskResult<Guid>> GetUserById(string auth0Id);
    Task<TaskResult<User>> CreateUser(UserVm user);
    Task<TaskResult<List<UserSearchVm>>> GetUserBySearchQuery(string searchQuery);
    Task<TaskResult<UserSearchVm>> UpdateUserStatus(UserSearchVm userSearchVm);
    Task<TaskResult<UserRoleDto>> GetUserRole(string auth0Id);
}