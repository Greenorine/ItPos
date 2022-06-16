using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ItPos.DataAccess.Handlers.Users;
using ItPos.Domain.Interfaces;
using ItPos.Domain.Models.User;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ItPos.Infrastructure.Services.Security;

public class JwtTokenManager : ITokenManager
{
    private readonly IMediator mediator;
    private readonly IConfiguration configuration;

    public JwtTokenManager(IMediator mediator, IConfiguration configuration)
    {
        this.mediator = mediator;
        this.configuration = configuration;
    }

    public string GenerateToken(IUser user, DateTime expiresAt)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Login),
            new(ClaimTypes.Expired, expiresAt.ToString("s")),
            new(CustomClaims.Group, user.Group)
        };

        var userRoles = user.Roles.Split(", ").ToList();
        authClaims.AddRange(userRoles.Select(r => new Claim(ClaimTypes.Role, r.ToString())));

        var userPermissions = user.Permissions?.Split(", ").ToList();
        if (userPermissions is not null)
        {
            authClaims.AddRange(userPermissions.Select(p =>
                new Claim(CustomClaims.Permission, p.ToString())));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"]!));
        var token = new JwtSecurityToken(
            null,
            null,
            expires: expiresAt,
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}