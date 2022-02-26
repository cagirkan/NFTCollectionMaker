using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class CollectionAnalytic
    {
        [Key]
        public int CollectionAnalyticID { get; set; }
        [StringLength(100)]
        public string Key { get; set; }
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
