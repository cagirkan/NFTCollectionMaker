using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ICollectionService : IGenericService<Collection>
    {
        public Collection GetCollectionsOfUser(string userName);
        public int AddWithReturn(Collection t);

    }
}
