using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Services;

/// <summary>
/// Orchestrates user-related business operations, ensuring data integrity and applying domain rules.
/// </summary>
public class UserService : IUserService
{
    private readonly UserDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(UserDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new user record after validating that the email address is unique within the system.
    /// </summary>
    /// <param name="request">The data required to create a user account.</param>
    /// <returns>A persistence-ready user profile mapped to a response DTO.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a user with the provided email already exists.</exception>
    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var existingUser = await _context.Users.AnyAsync(u => u.Email == request.Email);
        
        if (existingUser)
        {
            _logger.LogWarning("CreateUser failed: Email {Email} already exists", request.Email);
            throw new InvalidOperationException($"User with email '{request.Email}' already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("User created: {UserId}", user.Id);

        return MapToResponse(user);
    }

    /// <summary>
    /// Retrieves a user from the persistent store by their identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>A user profile if found; otherwise, <c>null</c>.</returns>
    public async Task<UserResponse?> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null ? null : MapToResponse(user);
    }

    /// <summary>
    /// Maps a domain model to a public-facing response DTO.
    /// </summary>
    private static UserResponse MapToResponse(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    };
}
