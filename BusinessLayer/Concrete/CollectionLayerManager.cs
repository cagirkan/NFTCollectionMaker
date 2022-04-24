using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class CollectionLayerManager : ICollectionLayerService
    {
        ICollectionLayerDal _collectionLayer;

        public CollectionLayerManager(ICollectionLayerDal collectionLayer)
        {
            _collectionLayer = collectionLayer;
        }

        public int AddWithReturn(CollectionLayer t)
        {
            _collectionLayer.Insert(t);
            //Get Last index
            return int.Parse(GetList().OrderByDescending(p => p.CollectionLayerID)
                        .Select(r => r.CollectionLayerID)
                        .First().ToString());
        }
        public void Add(CollectionLayer t)
        {
            _collectionLayer.Insert(t);
        }

        public void Delete(CollectionLayer t)
        {
            _collectionLayer.Delete(t);
        }

        public CollectionLayer GetByID(int id)
        {
            return _collectionLayer.GetByID(id);
        }

        public List<CollectionLayer> GetList()
        {
            return _collectionLayer.GetListAll();
        }

        public void Update(CollectionLayer t)
        {
            _collectionLayer.Update(t);
        }

        public List<CollectionLayer> GetLayersOfCollection(int collectionID)
        {
            return _collectionLayer.List(x => x.CollectionID == collectionID);
        }

        public List<List<string>> GetLayerPaths(List<CollectionLayer> collectionLayers)
        {
            List<CollectionLayer> orderedCollectionLayers = collectionLayers.OrderBy(x => x.LayerIndex).ToList();
            var typeList = new List<int>();
            List<List<string>> layerPaths = new List<List<string>>();

            foreach (var layer in orderedCollectionLayers)
            {
                if(typeList.Contains(layer.LayerTypeID))
                    layerPaths[typeList.IndexOf(layer.LayerTypeID)].Add(layer.ImagePath);
                else
                {
                    layerPaths.Add(new List<string> { layer.ImagePath });
                    typeList.Add(layer.LayerTypeID);
                }
            }
            return layerPaths;
        }
    }
}
