using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Security
{
    public class AuthenticationService
    {
        public UserResponse Authenticate(UserRequest user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("VGVzdGVzIGNvbSBBU1AuTkVUIDUgZSBKV1Q=");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Role, "admin")
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = "TestingIssuer",
                Audience = "TestingAudience",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new UserResponse()
            {
                Token = tokenHandler.WriteToken(token),
                ValidTo = token.ValidTo
            };
        }
    }

    

    public class UserResponse
    {
        public string? Token { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
