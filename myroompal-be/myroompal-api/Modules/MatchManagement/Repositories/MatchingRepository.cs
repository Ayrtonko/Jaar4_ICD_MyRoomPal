using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myroompal_api.Data;
using myroompal_api.Entities.Entities;
using myroompal_api.Modules.MatchManagement.Interfaces;
using myroompal_api.Modules.MatchManagement.Models;
using myroompal_api.Modules.MatchManagement.Models.Requests;
using myroompal_api.Modules.Shared;

namespace myroompal_api.Modules.MatchManagement.Repositories;

public class MatchingRepository : IMatchingRepository
{
    private readonly ApplicationDbContext _context;

    public MatchingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskResult<List<ProfileVm>>> GetUnlikedProfiles(string loggedInAuthId)
    {
        try
        {
            var loggedUser = await _context.Users.Include(user => user.Profile)
                .FirstOrDefaultAsync(e => e.Auth0Id == loggedInAuthId);
            if (loggedUser == null)
            {
                return TaskResult<List<ProfileVm>>.Failure("logged in user not found");
            }

            var loggedUserId = loggedUser.Id;
            var likedUserIds = await _context.Likes
                .Where(l => l.LikerUserId == loggedUserId)
                .Select(l => l.LikeeUserId)
                .ToListAsync();

            Console.WriteLine(
                $"{likedUserIds.Count}----------------------------------------------------------------------------------------------");

            var filteredUsers = await _context.Users
                .Include(user => user.Profile)
                .Include(user => user.PreferencesUsers)
                .Where(u => u.Profile.SearchLocation == loggedUser.Profile.SearchLocation
                            && u.Profile.UserId.Value != loggedUserId
                            && !likedUserIds.Contains(u.Id)
                )
                .Select(u => new ProfileVm
                {
                    UserId = u.Id,
                    FirstName = u.Profile.FirstName,
                    LastName = u.Profile.LastName,
                    Email = u.Profile.Email,
                    Gender = u.Profile.Gender,
                    BirthDate = u.Profile.BirthDate,
                    PhoneNumber = u.Profile.PhoneNumber,
                    SearchLocation = u.Profile.SearchLocation,
                    Preferences = u.PreferencesUsers.Select(e => e.PreferenceTag).ToList()
                })
                .ToListAsync();

            return TaskResult<List<ProfileVm>>.Success(filteredUsers);
        }
        catch (Exception e)
        {
            return TaskResult<List<ProfileVm>>.Failure(
                $"Error get users by location: {e.Message}, inner exception: {e.InnerException}");
        }
    }

    public async Task<TaskResult<ActionResult>> CreateLike(string loggedInAuthId, Guid likeeUserId)
    {
        try
        {
            var loggedUser = await _context.Users.FirstOrDefaultAsync(e => e.Auth0Id == loggedInAuthId);
            var likeeUser = await _context.Users.FirstOrDefaultAsync(e => e.Id == likeeUserId);
            if (loggedUser == null || likeeUser == null)
            {
                return TaskResult<ActionResult>.Failure("logged in user not found, or liked user not found");
            }

            var likerUserId = loggedUser.Id;
            var existingLike = await _context.Likes
                .AnyAsync(e => e.LikerUserId == likerUserId && e.LikeeUserId == likeeUserId
                );
            if (existingLike)
            {
                return TaskResult<ActionResult>.Failure("Like already exists.");
            }

            var like = new Like
            {
                LikerUser = loggedUser,
                LikerUserId = likerUserId,
                LikeeUserId = likeeUserId,
                LikeDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            var mutualLike =
                await _context.Likes.AnyAsync(e => e.LikerUserId == likeeUserId && e.LikeeUserId == likerUserId);

            if (mutualLike)
            {
                var match = new Match
                {
                    MatcherUserId = likerUserId,
                    MatcheeUserId = likeeUserId,
                    MatchDate = DateOnly.FromDateTime(DateTime.Now),
                    MatcherUser = loggedUser,
                    MatcheeUser = likeeUser
                };
                await _context.Matches.AddAsync(match);
                await _context.SaveChangesAsync();
            }

            return TaskResult<ActionResult>.Success(null!);
        }
        catch (Exception e)
        {
            return TaskResult<ActionResult>.Failure($"Error creating like for logged user in repository: {e.Message}");
        }
    }

    public async Task<TaskResult<List<ProfileVm>>> GetUserMatchProfiles(string loggedInAuthId)
    {
        try
        {
            var loggedUser = await _context.Users.FirstOrDefaultAsync(e => e.Auth0Id == loggedInAuthId);
            if (loggedUser == null)
            {
                return TaskResult<List<ProfileVm>>.Failure("logged in user not found");
            }

            var matchedProfiles = await _context.Matches
                .Where(m => m.MatcherUserId == loggedUser.Id || m.MatcheeUserId == loggedUser.Id)
                .Select(m => m.MatcherUserId == loggedUser.Id ? m.MatcheeUserId : m.MatcherUserId)
                .Join(_context.Profiles,
                    matchedUserId => matchedUserId,
                    profile => profile.UserId.Value,
                    (matchedUserId, profile) => new ProfileVm
                    {
                        UserId = profile.UserId.Value,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        Email = profile.Email,
                        Gender = profile.Gender,
                        BirthDate = profile.BirthDate,
                        PhoneNumber = profile.PhoneNumber,
                    })
                .ToListAsync();

            return TaskResult<List<ProfileVm>>.Success(matchedProfiles,
                $"Returned {matchedProfiles.Count} profile(s) from repository");
        }
        catch (Exception e)
        {
            return TaskResult<List<ProfileVm>>.Failure(
                $"Error get user match profiles for logged user in repository: {e.Message}");
        }
    }

    public async Task<TaskResult<ActionResult>> DeleteMatch(string loggedInAuthId, Guid likeeUserId)
    {
        try
        {
            var loggedUser = await _context.Users.FirstOrDefaultAsync(e => e.Auth0Id == loggedInAuthId);
            if (loggedUser == null)
            {
                return TaskResult<ActionResult>.Failure("Logged-in user not found");
            }

            var match = await _context.Matches.FirstOrDefaultAsync(m =>
                (m.MatcherUserId == loggedUser.Id && m.MatcheeUserId == likeeUserId) ||
                (m.MatcherUserId == likeeUserId && m.MatcheeUserId == loggedUser.Id));

            if (match == null)
            {
                return TaskResult<ActionResult>.Failure("Match not found");
            }

            _context.Matches.Remove(match);

            // Also remove the likes that created the match
            var likesToRemove = await _context.Likes
                .Where(l => (l.LikerUserId == loggedUser.Id && l.LikeeUserId == likeeUserId) ||
                            (l.LikerUserId == likeeUserId && l.LikeeUserId == loggedUser.Id))
                .ToListAsync();

            _context.Likes.RemoveRange(likesToRemove);

            await _context.SaveChangesAsync();

            return TaskResult<ActionResult>.Success(null!);
        }
        catch (Exception e)
        {
            return TaskResult<ActionResult>.Failure($"Error deleting match and associated likes: {e.Message}");
        }
    }

    public async Task<TaskResult<List<PreferenceVm>>> GetPreferences()
    {
        try
        {
            var preferences = await _context.Preferences
                .Select(p => new PreferenceVm
                {
                    Id = p.Id,
                    PreferenceTag = p.PreferenceTag,
                })
                .ToListAsync();

            return TaskResult<List<PreferenceVm>>.Success(preferences, "Returned preferences from repository");
        }
        catch (Exception e)
        {
            return TaskResult<List<PreferenceVm>>.Failure($"Error get preferences in repository: {e.Message}");
        }
    }

    public async Task<TaskResult<List<PreferenceVm>>> GetLoggedUserPreferences(string loggedInAuthId)
    {
        try
        {
            var loggedUser = await _context.Users.Include(e => e.PreferencesUsers)
                .FirstOrDefaultAsync(e => e.Auth0Id == loggedInAuthId);
            if (loggedUser == null)
            {
                return TaskResult<List<PreferenceVm>>.Failure("logged in user not found");
            }

            var preferences = loggedUser.PreferencesUsers.Select(e => new PreferenceVm
            {
                Id = e.Id,
                PreferenceTag = e.PreferenceTag,
            }).ToList();
            return TaskResult<List<PreferenceVm>>.Success(preferences);
        }
        catch (Exception e)
        {
            return TaskResult<List<PreferenceVm>>.Failure($"{e.Message}, {e.InnerException}");
        }
    }

    public async Task<TaskResult<ActionResult>> CreatePreferencesUser(PreferenceUserRequest request, string loggedInAuthId)
    {
        try
        {
            var loggedUser = await _context.Users.FirstOrDefaultAsync(e => e.Auth0Id == loggedInAuthId);
            if (loggedUser == null)
            {
                return TaskResult<ActionResult>.Failure("logged in user not found");
            }

            var preferenceUsersToDelete = _context.PreferenceUsers
                .Where(p => p.UserId == loggedUser.Id);

            if (preferenceUsersToDelete.Any())
            {
                _context.PreferenceUsers.RemoveRange(preferenceUsersToDelete);
            }

            foreach (var preference in request.Preferences)
            {
                var currentPreference = await _context.Preferences
                    .Where(x => x.Id.ToString() == preference)
                    .FirstOrDefaultAsync();

                if (currentPreference == null)
                {
                    return TaskResult<ActionResult>.Failure($"Preference tag {preference} not found in database");
                }

                var preferenceUser = new PreferenceUser
                {
                    PreferenceTag = currentPreference.PreferenceTag,
                    UserId = loggedUser.Id,
                    Preference = currentPreference,
                    User = loggedUser,
                };

                _context.PreferenceUsers.Add(preferenceUser);
            }

            await _context.SaveChangesAsync();
            return TaskResult<ActionResult>.Success(null!);
        }
        catch (Exception e)
        {
            return TaskResult<ActionResult>.Failure(
                $"Error creating preferences user in repository: {e.InnerException}");
        }
    }


    public async Task<TaskResult<ActionResult>> UpdateUserSearchLocation(SearchLocationRequest request, string loggedInAuthId)
    {
        try
        {
            var loggedUser = await _context.Users.Include(user => user.Profile)
                .FirstOrDefaultAsync(e => e.Auth0Id == loggedInAuthId);
            if (loggedUser == null)
            {
                return TaskResult<ActionResult>.Failure("logged in user not found");
            }

            loggedUser.Profile.SearchLocation = request.SearchLocation;
            await _context.SaveChangesAsync();
            return TaskResult<ActionResult>.Success(null!);
        }
        catch (Exception e)
        {
            return TaskResult<ActionResult>.Failure(
                $"Error updating SearchLocation in repository: {e.InnerException}");
        }
    }
}