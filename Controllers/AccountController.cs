using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos.Account;
using WebApi.Interfaces;
using WebApi.Model;

namespace WebApi.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenServices _tokenServices;
    private readonly SignInManager<AppUser> _signinManager;

    public AccountController(UserManager<AppUser> userManager, ITokenServices tokenServices, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenServices = tokenServices;
        _signinManager = signInManager;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var appUser = new AppUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email
        };

        var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

        if (!createdUser.Succeeded)
            return IdentityErrorHandler.HandleIdentityError(createdUser.Errors);

        var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
        if (!roleResult.Succeeded)
            return IdentityErrorHandler.HandleIdentityError(roleResult.Errors);

        return Ok(CreateNewUserDto(appUser));
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

        if (user == null) 
            return Unauthorized("Invalid username!");

        var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) 
            return Unauthorized("Username not found and/or password incorrect");

        return Ok(CreateNewUserDto(user));
    }

    private NewUserDto CreateNewUserDto(AppUser user)
    {
        return new NewUserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = _tokenServices.CreateToken(user)
        };
    }
}
