using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IArtworkService : IGenericService<Artwork>
    {
        public int AddWithReturn(Artwork t);
        public List<Artwork> GetByCollectionID(int id);
        public int GetLastID(int collectionID);
    }
}
