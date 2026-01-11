using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UserService.DTOs;

/// <summary>
/// Data transfer object for creating a new user.
/// </summary>
[SwaggerSchema(Description = "Request payload for creating a new user account")]
public class CreateUserRequest
{
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    [SwaggerSchema(Description = "Full name of the user")]
    [DefaultValue("John Doe")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [SwaggerSchema(Description = "Email address for the user account")]
    [DefaultValue("john.doe@example.com")]
    public string Email { get; set; } = string.Empty;
}
