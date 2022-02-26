using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class LayerTag
    {
        [Key]
        public int LayerTagID { get; set; }
        public int CollectionLayerID { get; set; }
        public CollectionLayer CollectionLayer { get; set; }
        public int TagID { get; set; }
        public Tag Tag { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
