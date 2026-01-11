using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Services;
using Asp.Versioning;

namespace UserService.Controllers;

/// <summary>
/// Handles user account management and lifecycle operations.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="request">The user creation request details.</param>
    /// <returns>The newly created user profile.</returns>
    /// <response code="201">Returns the newly created user.</response>
    /// <response code="400">If the request is invalid or the email already exists.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var response = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user profile if found.</returns>
    /// <response code="200">Returns the requested user.</response>
    /// <response code="404">If the user is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}
