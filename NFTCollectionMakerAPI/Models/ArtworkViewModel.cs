using EntityLayer.Concrete;
using System.Collections.Generic;

namespace NFTCollectionMakerAPI.Models
{
    public class ArtworkViewModel : Artwork
    {
        public List<string> Tags { get; set; }
    }
}
