using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SoleCode.Api.Common;

public interface IJwtAuthManager
{
    string GenerateTokens(string username, Claim[] claims, DateTime now);
    (ClaimsPrincipal principal, JwtSecurityToken?) DecodeJwtToken(string token);
}