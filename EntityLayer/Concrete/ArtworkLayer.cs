using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class ArtworkLayer
    {
        [Key]
        public int ArtworkLayerID { get; set; }
        public int ArtworkID { get; set; }
        public Artwork Artwork { get; set; }
        public int CollectionLayerID { get; set; }
        public CollectionLayer CollectionLayer { get; set; }
        public List<LayerTag> LayerTags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
