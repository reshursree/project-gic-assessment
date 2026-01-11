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
        // Idempotency check: Prevent duplicate emails
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (existingUser != null)
        {
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

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
        {
            return null;
        }

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}
