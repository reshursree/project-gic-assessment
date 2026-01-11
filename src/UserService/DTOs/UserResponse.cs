using Swashbuckle.AspNetCore.Annotations;

namespace UserService.DTOs;

/// <summary>
/// Data transfer object representing a user's profile information.
/// </summary>
[SwaggerSchema(Description = "User profile information returned after registration or retrieval")]
public class UserResponse
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [SwaggerSchema(Description = "Unique identifier (GUID) for the user")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    [SwaggerSchema(Description = "Full name of the user")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [SwaggerSchema(Description = "Email address of the user")]
    public string Email { get; set; } = string.Empty;
}
