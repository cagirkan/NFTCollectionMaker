using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IArtworkLayerService : IGenericService<ArtworkLayer>
    {
        public void DeleteCollectionLayersFromAL(int collectionLayerID);
        public void AddAnalytic(ArtworkLayer t, int collectionID, int layerTypeID);
    }
}
