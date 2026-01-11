using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Services;

/// <summary>
/// Defines the core business logic for user management.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Asynchronously creates a new user and persists it to the database.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <returns>A task representing the asynchronous operation, containing the created user response.</returns>
    Task<UserResponse> CreateUserAsync(CreateUserRequest request);
}

/// <summary>
/// Implementation of user management services using Entity Framework Core.
/// </summary>
public class UserService : IUserService
{
    private readonly UserDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserService(UserDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}
