using BusinessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly string key;
        readonly UserManager um = new UserManager(new EfUserRepository());

        public AuthManager(string key)
        {
            this.key = key;
        }

        public string Authenticate(string username, string password)
        {
            var users = um.GetList();
            var user = users.Find(x => x.UserName.Equals(username));
            if (user != null)
            {
                if (password.Equals(user.Password))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenKey = Encoding.ASCII.GetBytes(key);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, username),
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(tokenKey),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return tokenHandler.WriteToken(token);
                }
                else return null;
            }
            else return null;
        }
    }
}
