using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IArtworkTagService : IGenericService<ArtworkTag>
    {
        public List<int> GetTagsByID(int artworkID);
        public List<ArtworkTag> GetTagsOfArtworks(int artworkID);
    }
}
