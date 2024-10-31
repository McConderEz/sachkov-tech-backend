using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Accounts.Contracts.Responses;
using SachkovTech.Core.Abstractions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Application.Commands.RefreshTokens;

public record RefreshTokensCommand(Guid RefreshToken) : ICommand;

public class RefreshTokensHandler : ICommandHandler<LoginResponse, RefreshTokensCommand>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokensHandler(
        IRefreshSessionManager refreshSessionManager,
        ITokenProvider tokenProvider,
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork)
    {
        _refreshSessionManager = refreshSessionManager;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResponse, ErrorList>> Handle(
        RefreshTokensCommand command, CancellationToken cancellationToken = default)
    {
        var oldRefreshSessionResult = await _refreshSessionManager
            .GetByRefreshToken(command.RefreshToken, cancellationToken);

        if (oldRefreshSessionResult.IsFailure)
            return oldRefreshSessionResult.Error.ToErrorList();
        
        var oldRefreshSession = oldRefreshSessionResult.Value;

        if (oldRefreshSession.ExpiresIn < DateTime.UtcNow)
        {
            return Errors.Tokens.ExpiredToken().ToErrorList();
        }

        // var userClaims = await _tokenProvider.GetUserClaims(command.AccessToken, cancellationToken);
        // if (userClaims.IsFailure)
        // {
        //     return Errors.Tokens.InvalidToken().ToErrorList();
        // }
        //
        // var userIdString = userClaims.Value.FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;
        // if (!Guid.TryParse(userIdString, out var userId))
        // {
        //     return Errors.General.Failure().ToErrorList();
        // }

        // if (oldRefreshSession.Value.UserId != userId)
        // {
        //     return Errors.Tokens.InvalidToken().ToErrorList();
        // }

        // var userJtiString = userClaims.Value.FirstOrDefault(c => c.Type == CustomClaims.Jti)?.Value;
        // if (!Guid.TryParse(userJtiString, out var userJtiGuid))
        // {
        //     return Errors.General.Failure().ToErrorList();
        // }

        // if (oldRefreshSession.Value.Jti != userJtiGuid)
        // {
        //     return Errors.Tokens.InvalidToken().ToErrorList();
        // }

        _refreshSessionManager.Delete(oldRefreshSession);
        await _unitOfWork.SaveChanges(cancellationToken);

        var accessToken = await _tokenProvider
            .GenerateAccessToken(oldRefreshSession.User, cancellationToken);

        var refreshToken = await _tokenProvider
            .GenerateRefreshToken(oldRefreshSession.User, accessToken.Jti, cancellationToken);

        var roles = oldRefreshSession.User.Roles.Select(r => r.Name?.ToLower() ?? "").ToArray();
        
        return new LoginResponse(
            oldRefreshSession.User.Email ?? "",
            roles,
            accessToken.AccessToken,
            refreshToken);
    }
}