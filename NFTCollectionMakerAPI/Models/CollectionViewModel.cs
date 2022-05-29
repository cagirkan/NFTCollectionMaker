using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace NFTCollectionMakerAPI.Models
{
    public class CollectionViewModel
    {
        public int CollectionID { get; set; }
        public string CollectionName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string CoverImage { get; set; }
        public List<Artwork> Artworks { get; set; }
        public List<CollectionAnalytic> CollectionAnalytics { get; set; }
        public List<LayerType> LayerTypes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Tag { get; set; }
    }
}
