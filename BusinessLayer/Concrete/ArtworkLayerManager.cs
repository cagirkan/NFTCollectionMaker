using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class ArtworkLayerManager : IArtworkLayerService
    {
        readonly IArtworkLayerDal _artworkLayer;
        CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());
        LayerTypeManager ltm = new LayerTypeManager(new EfLayerTypeRepository());
        public ArtworkLayerManager(IArtworkLayerDal artworkLayer)
        {
            _artworkLayer = artworkLayer;
        }

        public void Add(ArtworkLayer t)
        {
            _artworkLayer.Insert(t);
        }
        public void AddAnalytic(ArtworkLayer t, int collectionID, int layerTypeID)
        {
            var layerType = ltm.GetByID(layerTypeID).LayerTypeName;
            cam.UpdateAnalytic(collectionID, Constants.Constants.Analytics.ArtworksWith + char.ToUpper(layerType[0]) + layerType.Substring(1), 1);
            _artworkLayer.Insert(t);
        }

        public void Delete(ArtworkLayer t)
        {
            _artworkLayer.Delete(t);
        }

        public ArtworkLayer GetByID(int id)
        {
            return _artworkLayer.GetByID(id);
        }

        public List<ArtworkLayer> GetList()
        {
            return _artworkLayer.GetListAll();
        }

        public void Update(ArtworkLayer t)
        {
            _artworkLayer.Update(t);
        }

        public List<int> GetLayersOfArtwork(int artworkID)
        {
            List<int> layers = new List<int>();
            var artworkLayers = _artworkLayer.List(x => x.ArtworkID == artworkID);
            foreach (var item in artworkLayers)
            {
                layers.Add(item.CollectionLayerID);
            }
            return layers;
        }

        public void DeleteCollectionLayersFromAL(int collectionLayerID)
        {
            var layers = _artworkLayer.List(x => x.CollectionLayerID == collectionLayerID);
            foreach (var item in layers)
            {
                Delete(item);
            }
        }
    }
}
