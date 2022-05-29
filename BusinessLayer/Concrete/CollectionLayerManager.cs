using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BusinessLayer.Concrete
{
    public class CollectionLayerManager : ICollectionLayerService
    {
        readonly ICollectionLayerDal _collectionLayer;
        readonly CollectionManager cm = new CollectionManager(new EfCollectionRepository());
        readonly CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());

        public CollectionLayerManager(ICollectionLayerDal collectionLayer)
        {
            _collectionLayer = collectionLayer;
        }

        public int AddWithReturn(CollectionLayer t)
        {
            cam.UpdateAnalytic(t.CollectionID, Constants.Constants.Analytics.LayerItems, 1);
            _collectionLayer.Insert(t);
            //Get Last index
            return int.Parse(GetList().OrderByDescending(p => p.CollectionLayerID)
                        .Select(r => r.CollectionLayerID)
                        .First().ToString());
        }
        public void Add(CollectionLayer t)
        {
            cam.UpdateAnalytic(t.CollectionID, Constants.Constants.Analytics.LayerItems, 1);
            _collectionLayer.Insert(t);
        }

        public void Delete(CollectionLayer t)
        {
            cam.UpdateAnalytic(t.CollectionID, Constants.Constants.Analytics.LayerItems, -1);
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

        public List<CollectionLayer> GetCollectionLayersOfUser(int userID)
        {
            List<CollectionLayer> collectionLayers = GetList();
            List<CollectionLayer> userCollectionLayers = new List<CollectionLayer>();
            foreach (var item in collectionLayers)
            {
                if (cm.GetByID(item.CollectionID).UserId == userID)
                {
                    userCollectionLayers.Add(item);
                }
            }
            return userCollectionLayers;
        }

        public void DeleteLayersOfType(int id)
        {
            List<CollectionLayer> collectionLayers = _collectionLayer.List(x => x.LayerTypeID == id);
            foreach (var item in collectionLayers)
            {
                _collectionLayer.Delete(item);
            }
        }

        public CollectionLayer GetByIDAuth(int id, int userID)
        {
            CollectionLayer collectionLayer = _collectionLayer.GetByID(id);
            if (cm.GetByID(collectionLayer.CollectionID).UserId == userID)
                return collectionLayer;
            else
                return null;
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

        public List<List<CollectionLayer>> GetCollectionLayersByType(List<CollectionLayer> collectionLayers)
        {
            List<CollectionLayer> orderedCollectionLayers = collectionLayers.OrderBy(x => x.LayerIndex).ToList();
            var typeList = new List<int>();
            List<List<CollectionLayer>> colLayers = new List<List<CollectionLayer>>();

            foreach (var layer in orderedCollectionLayers)
            {
                if (typeList.Contains(layer.LayerTypeID))
                    colLayers[typeList.IndexOf(layer.LayerTypeID)].Add(layer);
                else
                {
                    colLayers.Add(new List<CollectionLayer> { layer });
                    typeList.Add(layer.LayerTypeID);
                }
            }
            return colLayers;
        }

        //UNUSED
        public List<List<int>> GetCollectionLayerIDList(List<CollectionLayer> collectionLayers)
        {
            List<CollectionLayer> orderedCollectionLayers = collectionLayers.OrderBy(x => x.LayerIndex).ToList();
            var typeList = new List<int>();
            List<List<int>> collectionIDList = new List<List<int>>();

            foreach (var layer in orderedCollectionLayers)
            {
                if (typeList.Contains(layer.LayerTypeID))
                    collectionIDList[typeList.IndexOf(layer.LayerTypeID)].Add(layer.CollectionLayerID);
                else
                {
                    collectionIDList.Add(new List<int> { layer.CollectionLayerID });
                    typeList.Add(layer.LayerTypeID);
                }
            }
            return collectionIDList;
        }

        public Bitmap CreateBitmap(CollectionLayer collectionLayer)
        {
            return new Bitmap(collectionLayer.ImagePath);
        }
    }
}
