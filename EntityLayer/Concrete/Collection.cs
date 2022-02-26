using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    class Collection
    {
        [Key]
        public int CollectionID { get; set; }
        [StringLength(50)]
        public string CollectionName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
