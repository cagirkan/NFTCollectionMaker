using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class LayerTypeManager : ILayerTypeService
    {
        ILayerTypeDal _layerType;
        CollectionManager cm = new CollectionManager(new EfCollectionRepository());

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

        public LayerType GetByIDAuth(int id, int userID)
        {
            LayerType layerType = _layerType.GetByID(id);
            if (cm.GetByID(layerType.CollectionID).UserId == userID)
                return layerType;
            else
                return null;
        }

        public List<LayerType> GetLayerTypesOfUser(int userID)
        {
            List<LayerType> layerTypes = GetList();
            List<LayerType> userLayerTypes = new List<LayerType>();
            foreach (var item in layerTypes)
            {
                if (cm.GetByID(item.CollectionID).UserId == userID)
                {
                    userLayerTypes.Add(item);
                }
            }
            return userLayerTypes;
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
