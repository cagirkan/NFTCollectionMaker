using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        [StringLength(50)]
        public string TagName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
