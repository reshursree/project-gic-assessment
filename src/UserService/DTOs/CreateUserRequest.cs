namespace UserService.DTOs;

/// <summary>
/// Data transfer object for creating a new user.
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
