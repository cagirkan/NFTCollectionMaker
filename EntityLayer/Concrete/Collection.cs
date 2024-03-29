﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class Collection
    {
        [Key]
        public int CollectionID { get; set; }
        [StringLength(50)]
        public string CollectionName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string CoverImage { get; set; }
        public List<Artwork> Artworks { get; set; }
        public List<CollectionAnalytic> CollectionAnalytics { get; set; }
        public List<CollectionLayer> CollectionLayers { get; set; }
        public List<LayerType> LayerTypes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
