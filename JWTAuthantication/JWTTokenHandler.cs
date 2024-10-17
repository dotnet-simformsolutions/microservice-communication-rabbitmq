using JWTAuthantication.Entities;
using JWTAuthantication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthantication
{
    public class JWTTokenHandler
    {
        public const string JWT_Key = "this is my custom Secret key for authentication";
        private const int JWT_Token_Valadity = 20;
        private Ecommerce_userDBContext _ecommerce_UserDBContext;

        public JWTTokenHandler(Ecommerce_userDBContext ecommerce_UserDBContext)
        {
            _ecommerce_UserDBContext = ecommerce_UserDBContext;
        }

        public AuthenticationResponse? GenerateToken(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrWhiteSpace(authenticationRequest.Username) || string.IsNullOrWhiteSpace(authenticationRequest.Password))
                return null;

            var user = _ecommerce_UserDBContext.Users.Where(x => x.UserName == authenticationRequest.Username && x.Password == authenticationRequest.Password).FirstOrDefault();
            if (user == null)
                return null;

            var expireTimeStamp = DateTime.Now.AddMinutes(JWT_Token_Valadity);
            var tokenkey = Encoding.ASCII.GetBytes(JWT_Key);

            var claims = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authenticationRequest.Username),
                new Claim("Role", user.Role),
            });

            var signingCredential = new SigningCredentials(
                new SymmetricSecurityKey(tokenkey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expireTimeStamp,
                SigningCredentials = signingCredential
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                UserName = user.UserName,
                ExpireIn = (int)expireTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token
            };
        }
    }
}
