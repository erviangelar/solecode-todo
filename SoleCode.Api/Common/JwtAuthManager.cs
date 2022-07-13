using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SoleCode.Api.Common;

public class JwtAuthManager : IJwtAuthManager
{
    private readonly byte[] _secret;

    public JwtAuthManager()
    {
        _secret = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JwtKey") ?? throw new InvalidOperationException("Invalid JWT Key"));
    }

    public string GenerateTokens(string username, Claim[] claims, DateTime now)
    {
        var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
        var jwtToken = new JwtSecurityToken(
            Environment.GetEnvironmentVariable("JwtIssuer"),
            shouldAddAudienceClaim ? Environment.GetEnvironmentVariable("JwtAudience") : string.Empty,
            claims,
            expires: now.AddHours(Convert.ToInt32(Environment.GetEnvironmentVariable("JwtTokenDuration"))),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return accessToken;
    }

    public (ClaimsPrincipal principal, JwtSecurityToken?) DecodeJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new SecurityTokenException("Invalid token");
        }
        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JwtIssuer"),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidAudience = Environment.GetEnvironmentVariable("JwtAudience"),
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                },
                out var validatedToken);
        return (principal, validatedToken as JwtSecurityToken);
    }

}