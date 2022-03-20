using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IAuthService
    {
        public string Authenticate(string username, string password);
    }
}
