using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenkey = config["TokenKey"] ?? throw new  Exception ("Cannot access tokenkey from appsettings");
        if(tokenkey.Length < 64) throw new Exception("Your token need to be longer");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.UserName)

        };
        var creds= new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds

        };

        var tokenhandler = new JwtSecurityTokenHandler();
        var token = tokenhandler.CreateToken(tokenDescriptor);

        return tokenhandler.WriteToken(token);

    }
}
