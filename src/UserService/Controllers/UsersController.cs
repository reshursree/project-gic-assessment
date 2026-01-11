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
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="logger">The logger.</param>
    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
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
        _logger.LogInformation("Creating user with email: {Email}", request.Email);
        
        try
        {
            var response = await _userService.CreateUserAsync(request);
            _logger.LogInformation("User created successfully with ID: {UserId}", response.Id);
            return CreatedAtAction(nameof(GetUser), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Failed to create user with email {Email}: {Error}", request.Email, ex.Message);
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
        _logger.LogInformation("Retrieving user with ID: {UserId}", id);
        
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", id);
            return NotFound();
        }

        _logger.LogInformation("User retrieved successfully: {UserId}", id);
        return Ok(user);
    }
}
