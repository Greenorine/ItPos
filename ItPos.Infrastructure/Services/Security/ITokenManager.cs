using ItPos.Domain.Interfaces;

namespace ItPos.Infrastructure.Services.Security;

public interface ITokenManager
{
    public string GenerateToken(IUser user, DateTime expiresAt);
}