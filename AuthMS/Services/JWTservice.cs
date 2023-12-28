using AuthMS.Models;
using AuthMS.Services.Iservice;
using AuthMS.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthMS.Services
{
    public class JWTservice : IJWT
    {
        private readonly JWToptions _jwtoptions;

        public JWTservice(IOptions<JWToptions> jwtoptions)
        {
            _jwtoptions = jwtoptions.Value;


        }
        public string GetToken(ApplicationUser user, IEnumerable<string> Roles)
        { 
            // Creating Credentials 

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtoptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Add the Payload 

            List<Claim> Claims = new List<Claim>();

            Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));


            // Adding the List of Roles to our claims .

            Claims.AddRange(Roles.Select(x=> new Claim(ClaimTypes.Role, x)));


            // Combining everything together [credentials , claims , Issuer and Audience]

            var tokendescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtoptions.Issuer,
                Audience = _jwtoptions.Audience,
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(3),
                Subject = new ClaimsIdentity(Claims)

            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokendescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
