using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Services;

/// <summary>
/// Implementation of user management services using Entity Framework Core.
/// </summary>
public class UserService : IUserService
{
    private readonly UserDbContext _context;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="logger">The logger.</param>
    public UserService(UserDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        _logger.LogDebug("Checking for existing user with email: {Email}", request.Email);
        
        // Idempotency check: Prevent duplicate emails
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (existingUser != null)
        {
            _logger.LogWarning("Duplicate email detected: {Email}", request.Email);
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
        
        _logger.LogInformation("User created in database: {UserId}, {Email}", user.Id, user.Email);

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid id)
    {
        _logger.LogDebug("Querying database for user: {UserId}", id);
        
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
        {
            _logger.LogDebug("User not found in database: {UserId}", id);
            return null;
        }

        _logger.LogDebug("User found in database: {UserId}", id);
        
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}
