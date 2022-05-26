using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ICollectionAnalyticService : IGenericService<CollectionAnalytic>
    {
        public void InitializeAnalytics(int collectionID);
        public void UpdateAnalytic(int collectionID, string key, int value);
    }
}
