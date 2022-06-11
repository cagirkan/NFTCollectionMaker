using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class LayerTagManager : ILayerTagService
    {
        readonly ILayerTagDal _layerTag;
        readonly TagManager tm = new TagManager(new EfTagRepository());
        readonly CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());
        public LayerTagManager(ILayerTagDal layerTag)
        {
            _layerTag = layerTag;
        }

        public void Add(LayerTag t)
        {
            t.CreatedAt = DateTime.Now;
            t.UpdatedAt = DateTime.Now;
            cam.UpdateAnalytic(t.CollectionLayer.CollectionID, Constants.Constants.Analytics.LayerItems, 1);
            _layerTag.Insert(t);
        }

        public void Delete(LayerTag t)
        {
            _layerTag.Delete(t);
        }

        public LayerTag GetByID(int id)
        {
            return _layerTag.GetByID(id);
        }

        public List<LayerTag> GetList()
        {
            return _layerTag.GetListAll();
        }

        public int GetTagIDofCollection(int collectiomLayerID)
        {
            return _layerTag.Get(x => x.CollectionLayerID == collectiomLayerID).TagID;
        }

        //public string GetTagOfCollection(int collectiomLayerID)
        //{
        //    var tagID = GetTagIDofCollection(collectiomLayerID); 
        //    return tm.GetByID(tagID).TagName;
        //}

        public void Update(LayerTag t)
        {
            _layerTag.Update(t);
        }

        public List<string> getTagsOfArtwork(List<CollectionLayer> collectionLayers)
        {
            List<string> tagsOfArtwork = new List<string>();
            foreach (CollectionLayer layer in collectionLayers)
            {
                int tagID = _layerTag.Get(x => x.CollectionLayerID == layer.CollectionLayerID).TagID;
                tagsOfArtwork.Add(tm.GetByID(tagID).TagName);
            }
            return tagsOfArtwork;
        }
    }
}
