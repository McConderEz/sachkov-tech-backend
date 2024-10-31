using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Accounts.Application.Commands.EnrollParticipant;
using SachkovTech.Accounts.Application.Commands.Login;
using SachkovTech.Accounts.Application.Commands.RefreshTokens;
using SachkovTech.Accounts.Application.Commands.Register;
using SachkovTech.Accounts.Contracts.Requests;
using SachkovTech.Framework;
using SachkovTech.Framework.Authorization;

namespace SachkovTech.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    [HttpGet("test")]
    [Permission(Permissions.Issues.ReadIssue)]
    public async Task<IActionResult> Test(
        CancellationToken cancellationToken)
    {
        return Ok("test");
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new RegisterUserCommand(request.Email, request.UserName, request.Password, request.FullName),
            cancellationToken);
    
        if (result.IsFailure)
            return result.Error.ToResponse();
    
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new LoginCommand(request.Email, request.Password),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        // var cookieOptions = new CookieOptions
        // {
        //     HttpOnly = true,
        //     Secure = false, // Установите true, если переходите на HTTPS
        //     SameSite = SameSiteMode.None,
        //     Expires = DateTime.Now.AddDays(1)
        // };

        Response.Cookies.Append("refresh_token", result.Value.RefreshToken.ToString());

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens(
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("refresh_token", out var myCookieValue))
        {
            return Unauthorized();
        }
        
        var result = await handler.Handle(
            new RefreshTokensCommand(Guid.Parse(myCookieValue)),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        // var cookieOptions = new CookieOptions
        // {
        //     HttpOnly = true,
        //     Secure = false, // Установите true, если переходите на HTTPS
        //     SameSite = SameSiteMode.None,
        //     Expires = DateTime.Now.AddDays(1)
        // };
        
        Response.Cookies.Append("refresh_token", result.Value.RefreshToken.ToString());

        return Ok(result.Value);
    }

    [Permission(Permissions.Accounts.EnrollAccount)]
    [HttpPut("student-role")]
    public async Task<ActionResult> EnrollParticipant(
        [FromBody] EnrollParticipantRequest request,
        [FromServices] EnrollParticipantHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new EnrollParticipantCommand(request.Email);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}