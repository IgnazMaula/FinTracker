using FinTracker.Application.Interfaces;
using FinTracker.Application.Services;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public AuthController(IConfiguration configuration, IAuthService authService, IUserRepository userRepository)
    {
        _configuration = configuration;
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var token = await _authService.AuthenticateAsync(login.Username, login.Password);
        if (token == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(token);
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (await _userRepository.GetUserByUsernameAsync(model.Username) != null)
        {
            return BadRequest("Username already exists.");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = model.Username,
            PasswordHash = hashedPassword,
            Role = "User",
            Email = model.Email
        };

        await _userRepository.CreateUserAsync(user);

        return CreatedAtAction(nameof(UserController.GetUserById), "User", new { id = user.Id }, user);
    }

}
