using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class CollectionLayer
    {
        [Key]
        public int CollectionLayerID { get; set; }
        [StringLength(50)]
        public string CollectionLayerName { get; set; }
        [StringLength(512)]
        public string ImageURL { get; set; }
        public int LayerIndex { get; set; }
        public int Popularity { get; set; }
        public List<ArtworkLayer> ArtworkLayers { get; set; }
        public int CollectionID { get; set; }
        public Collection Collection { get; set; }
        public int LayerTypeID { get; set; }
        public LayerType LayerType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        

    }
}
