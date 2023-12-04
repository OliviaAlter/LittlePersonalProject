using Identity.Mapping;
using Identity.Request.Token;
using Identity.Request.User;
using IdentityApplication.ServiceInterface.User;
using IdentityCore.ServiceInterface.Token;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controller;

[Route("api/auth")]
[ApiController]
public class IdentityController(IEndUserService endUserService, ITokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] EndUserRegisterRequest request)
    {
        var mappedUser = request.MapToUser();

        var result = await endUserService
            .RegisterAsync(mappedUser);

        if (result is null)
            throw new BadHttpRequestException("User registration failed.");

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> GenerateToken([FromBody] UserLoginRequest request)
    {
        var (jwtToken, refreshToken) = await endUserService
            .LoginAsync(request.EmailOrUsername, request.Password);

        if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
            throw new BadHttpRequestException("Invalid client request");

        return Ok(new
        {
            Token = jwtToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenGenerationRequest request)
    {
        var mappedToken = request.MapToTokenGeneration();

        // Generate new tokens
        var (newJwtToken, newRefreshToken) = await tokenService.RefreshTokenForUser(mappedToken);

        if (string.IsNullOrEmpty(newJwtToken) || string.IsNullOrEmpty(newRefreshToken))
            throw new BadHttpRequestException("Invalid client request");

        // Return the new tokens
        return Ok(new
        {
            jwtToken = newJwtToken,
            refreshToken = newRefreshToken
        });
    }
}