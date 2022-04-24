using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ICollectionLayerService : IGenericService<CollectionLayer>
    {
        public int AddWithReturn(CollectionLayer t);
        public List<CollectionLayer> GetLayersOfCollection(int collectionID);
        public List<List<string>> GetLayerPaths(List<CollectionLayer> collectionLayers);
    }
}
