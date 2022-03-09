using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        //Auth işlemleri sona saklanacak
        public bool isUnique(string name, string email);
    }
}
