using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class ArtworkTag
    {
        [Key]
        public int MyProperty { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
