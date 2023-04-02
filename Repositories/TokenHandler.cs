using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UdemyProject.Models.Domain;

namespace UdemyProject.Repositories
{
    public class TokenHandler: ITokenHandler
    {
        private readonly IConfiguration _configuration;
        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<string> CreateTokenAsync (UsersDomain userDomain)
        {

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.GivenName, userDomain.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, userDomain.LastName));
            claims.Add(new Claim(ClaimTypes.Email, userDomain.EmailAddress));

            userDomain.Roles.ForEach((role) =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            // System.Security.Claims.Claim implement claims-based identity in .NET;
            // Claims represent attributes of the subject (user) that are useful in the context of authentication and authorization operations. 

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // Encoding the valus of JWT:KEY saved in the appsetting.json

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // credentials represent the digital signature produced via signingcredentials class

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
            // returned the complete token and passed it to the auth controllers
        }
    } 
}
