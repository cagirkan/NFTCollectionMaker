using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer.Concrete
{
    class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
