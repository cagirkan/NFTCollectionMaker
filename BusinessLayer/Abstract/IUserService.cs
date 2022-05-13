using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        public bool isUsernameUnique(string name);
        public bool isEmailUnique(string email);
        public int getIdByUsername(string username);
        public string GetUserName(string token);
        public User GetUser(string token);
    }
}
