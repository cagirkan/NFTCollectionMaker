using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class LayerTypeManager : ILayerTypeService
    {
        ILayerTypeDal _layerType;

        public LayerTypeManager(ILayerTypeDal layerType)
        {
            _layerType = layerType;
        }

        public void Add(LayerType t)
        {
            _layerType.Insert(t);
        }

        public void Delete(LayerType t)
        {
            _layerType.Delete(t);
        }

        public List<LayerType> GetByCollectionID(int id)
        {
            return _layerType.List(x => x.CollectionID == id);
        }

        public LayerType GetByID(int id)
        {
            return _layerType.GetByID(id);
        }

        public List<LayerType> GetList()
        {
            return _layerType.GetListAll();
        }

        public void Update(LayerType t)
        {
            _layerType.Update(t);
        }
    }
}
