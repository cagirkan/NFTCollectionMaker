using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [StringLength(25)]
        public string UserName { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Collection> Collections { get; set; }

    }
}
