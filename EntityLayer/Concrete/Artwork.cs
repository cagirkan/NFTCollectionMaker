﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class Artwork
    {
        [Key]
        public int ArtworkID { get; set; }
        [StringLength(50)]
        public string ArtworkName { get; set; }
        [StringLength(512)]
        public string ImageURL { get; set; }
        public int CollectionID { get; set; }
        public Collection Collection { get; set; }
        public List<ArtworkLayer> ArtworkLayers { get; set; }
        public List<ArtworkTag> ArtworkTags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
