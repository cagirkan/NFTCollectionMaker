using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _user;

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
    }
}
