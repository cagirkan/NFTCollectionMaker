using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IArtworkService : IGenericService<Artwork>
    {
        public List<Artwork> GetByCollectionID(int id);
        public int GetLastID(int collectionID);
    }
}
