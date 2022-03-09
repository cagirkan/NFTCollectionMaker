using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    interface ICollectionService : IGenericService<Collection>
    {
        public List<Collection> GetCollectionsOfUser(int userID);
    }
}
