using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class LayerType
    {
        [Key]
        public int LayerTypeID { get; set; }
        [StringLength(25)]
        public string LayerTypeName { get; set; }
        public int CollectionID { get; set; }
        public Collection Collection { get; set; }
        public List<CollectionLayer> CollectionLayers { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
