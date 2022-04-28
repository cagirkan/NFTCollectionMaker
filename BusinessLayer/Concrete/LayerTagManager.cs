using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class LayerTagManager : ILayerTagService
    {
        ILayerTagDal _layerTag;

        public LayerTagManager(ILayerTagDal layerTag)
        {
            _layerTag = layerTag;
        }

        public void Add(LayerTag t)
        {
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

        public void Update(LayerTag t)
        {
            _layerTag.Update(t);
        }
    }
}
