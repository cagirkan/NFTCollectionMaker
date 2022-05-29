using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ICollectionLayerService : IGenericService<CollectionLayer>
    {
        public int AddWithReturn(CollectionLayer t);
        public List<CollectionLayer> GetLayersOfCollection(int collectionID);
        public List<List<string>> GetLayerPaths(List<CollectionLayer> collectionLayers);
        public List<List<int>> GetCollectionLayerIDList(List<CollectionLayer> collectionLayers);
        public List<List<CollectionLayer>> GetCollectionLayersByType(List<CollectionLayer> collectionLayers);
        public List<CollectionLayer> GetCollectionLayersOfUser(int userID);
        public CollectionLayer GetByIDAuth(int id, int userID);
        public void DeleteLayersOfType(int id);
    }
}
