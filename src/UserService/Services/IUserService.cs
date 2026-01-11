using UserService.DTOs;

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

    /// <summary>
    /// Asynchronously retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A task representing the asynchronous operation, containing the user response if found, otherwise null.</returns>
    Task<UserResponse?> GetUserByIdAsync(Guid id);
}
