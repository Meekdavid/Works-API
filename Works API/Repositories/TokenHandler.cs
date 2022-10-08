using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Works_API.Models.Domain;

namespace Works_API.Repositories
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration configuration;
        public TokenHandler()
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(User user)
        {
           // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            //Create Claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
            claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress));


            // Loop into roles of users to add role to claims
            user.Roles.ForEach((role) => claims.Add(new Claim(ClaimTypes.Role, role)));

            // Create a Token
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("dwc8aqu8ExmFfJzKLudJ4DpefeE7dDAMBhSXexM"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                "https://localhost:7033",
                "https://localhost:7033",
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

          return  new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
