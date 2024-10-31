namespace SachkovTech.Accounts.Contracts.Responses;

public record LoginResponse(string Email, string[] Roles, string AccessToken, Guid RefreshToken);