using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Services;
using Asp.Versioning;

namespace UserService.Controllers;

/// <summary>
/// Provides administrative and self-service endpoints for managing user accounts and profiles.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Registers a new user within the system.
    /// </summary>
    /// <param name="request">The user registration details including unique email and full name.</param>
    /// <returns>A newly created user profile with a system-assigned unique identifier.</returns>
    /// <response code="201">Successfully created the user and returned the new profile.</response>
    /// <response code="400">The request was invalid (e.g., malformed data) or the email already exists.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var response = await _userService.CreateUserAsync(request);
        return CreatedAtAction(nameof(GetUser), new { id = response.Id }, response);
    }

    /// <summary>
    /// Retrieves a specific user profile by its unique identifier.
    /// </summary>
    /// <param name="id">The unique GUID of the user record.</param>
    /// <returns>The detailed user profile information if the record exists.</returns>
    /// <response code="200">The user profile was successfully retrieved.</response>
    /// <response code="404">The requested user identifier does not exist in the system.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }
}
