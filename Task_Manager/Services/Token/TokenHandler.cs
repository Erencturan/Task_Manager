using Microsoft.IdentityModel.Tokens;
using static System.Net.Mime.MediaTypeNames;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Task_Manager.Infrastructure.Models;
using Task_Manager.Core.Abstract.Token;

namespace Task_Manager.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DTOs.Token CreateAccessToken(int minute, User appUser)
        {
            DTOs.Token token = new DTOs.Token();

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes("ErenCanTuranSecretKeyTaskManager"));

            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.UtcNow.AddMinutes(minute);
            JwtSecurityToken securityToken = new(
                    audience: _configuration["Token:Audience"],
                    issuer: _configuration["Token:Issuer"],
                    expires: token.Expiration,
                    notBefore: DateTime.UtcNow,
                    signingCredentials: signingCredentials,
                    claims: new List<Claim> { new(ClaimTypes.Name, appUser.UserName), new(ClaimTypes.NameIdentifier, appUser.Id) }
               );

            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);


            return token;

        }

    

 
    }
}
