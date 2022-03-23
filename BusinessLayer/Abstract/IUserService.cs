using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        public bool isUsernameUnique(string name);
        public bool isEmailUnique(string email);
    }
}
