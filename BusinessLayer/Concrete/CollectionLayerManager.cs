using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CollectionLayerManager : ICollectionLayerService
    {
        readonly ICollectionLayerDal _collectionLayer;
        readonly CollectionManager cm = new CollectionManager(new EfCollectionRepository());
        readonly CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());
        readonly ArtworkLayerManager alm = new ArtworkLayerManager(new EfArtworkLayerRepository());
        readonly LayerTypeManager ltm = new LayerTypeManager(new EfLayerTypeRepository());

        public CollectionLayerManager(ICollectionLayerDal collectionLayer)
        {
            _collectionLayer = collectionLayer;
        }

        public async Task<int> AddWithReturn(CollectionLayer t)
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
            if ((System.IO.File.Exists(t.ImagePath)))
            {
                System.IO.File.Delete(t.ImagePath);
            }
            cam.UpdateAnalytic(t.CollectionID, Constants.Constants.Analytics.LayerItems, -1);
            alm.DeleteCollectionLayersFromAL(t.CollectionLayerID);
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
            if(collectionLayers != null)
            {
                foreach (var item in collectionLayers)
                {
                    Delete(item);
                }
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

        public List<string> getTypesOfArtwork(List<int> collectionLayers)
        {
            List<string> typesOfArtwork = new List<string>();
            foreach (int layerID in collectionLayers)
            {
                var collectionLayer = _collectionLayer.GetByID(layerID);
                var type = ltm.GetByID(collectionLayer.LayerTypeID).LayerTypeName;
                typesOfArtwork.Add(type);
            }
            return typesOfArtwork;
        }
    }
}
