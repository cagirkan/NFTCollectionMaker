using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        readonly IUserDal _user;

        public UserManager(IUserDal user)
        {
            _user = user;
        }

        public void Add(User t)
        {
            _user.Insert(t);
        }

        public void Delete(User t)
        {
            _user.Delete(t);
        }

        public User GetByID(int id)
        {
            return _user.GetByID(id);
        }

        public int getIdByUsername(string username)
        {
            return _user.Get(x => x.UserName == username).UserID;
        }

        public List<User> GetList()
        {
            return _user.GetListAll();
        }

        public bool isEmailUnique(string email)
        {
            User user = _user.Get(x => x.Email.Equals(email));
            if (user == null)
                return true;
            return false;
        }

        public bool isUsernameUnique(string name)
        {
            User user = _user.Get(x => x.UserName.Equals(name));
            if (user == null)
                return true;
            return false;
        }

        public void Update(User t)
        {
            _user.Update(t);
        }

        public string GetUserName(string token)
        {
            var key = Encoding.ASCII.GetBytes(ContextSettings.JWTKey);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return claims.Identity.Name;
        }

        public User GetUser(string token)
        {
            var key = Encoding.ASCII.GetBytes(ContextSettings.JWTKey);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            var uID = getIdByUsername(claims.Identity.Name);
            return GetByID(uID);
        }
    }
}
