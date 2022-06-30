using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Models.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repositories
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration configuration;

        public TokenHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task<string> CreateTokenAsync(Users users)
        {        
            //Create the claims for this token
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.GivenName, users.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, users.LastName));
            claims.Add(new Claim(ClaimTypes.Email, users.Email));

            // Loop into roles of users
            users.Roles.ForEach((role) =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credetials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credetials);
                
            return Task.FromResult( new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
