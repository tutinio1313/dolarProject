using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Extensions.Configuration;

namespace backend.Helper;

public class TokenHelper(IConfiguration config) : ITokenHelper
{
   private SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(config["ClientSecret"]!));
    private Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler tokenHandler = new(){SetDefaultTimesOnTokenCreation = false};


    public async Task<bool> ValidateTokenAsync(string token)
    {
        TokenValidationParameters parameters = new()
        {
          ValidIssuer = config["Jwt:Issuer"],
          ValidAudience = config["Jwt:Audience"],
          
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey =  key
        };

        var result = await tokenHandler.ValidateTokenAsync(token, parameters);
        return result.IsValid;
    
    }
    public string GenerateToken(string username)
    {
        Random random = new();
        Dictionary<string,object> values = new()
        {
            [ClaimTypes.Name] = username
            ,[ClaimTypes.Sid] = random.NextInt64().ToString()
            ,[ClaimTypes.GroupSid] = Guid.NewGuid()
        };
        SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);
        SecurityTokenDescriptor token = new()
    {
            SigningCredentials = signingCredentials
            ,Claims = values
            , Issuer = config["Jwt:Issuer"]
            , Audience = config["Jwt:Audience"]
            , Expires = DateTime.UtcNow.AddHours(3)
            , CompressionAlgorithm = "HS256"
    };
        return tokenHandler.CreateToken(token);
    }    
}