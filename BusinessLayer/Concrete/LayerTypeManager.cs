using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class LayerTypeManager : ILayerTypeService
    {
        readonly ILayerTypeDal _layerType;
        readonly CollectionManager cm = new CollectionManager(new EfCollectionRepository());
        readonly CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());

        public LayerTypeManager(ILayerTypeDal layerType)
        {
            _layerType = layerType;
        }

        public void Add(LayerType t)
        {
            CollectionAnalytic collectionAnalytic = new CollectionAnalytic();
            collectionAnalytic.CollectionID = t.CollectionID;
            collectionAnalytic.Key =Constants.Constants.Analytics.ArtworksWith + char.ToUpper(t.LayerTypeName[0]) + t.LayerTypeName.Substring(1);
            collectionAnalytic.Value = 0;
            cam.Add(collectionAnalytic);
            cam.UpdateAnalytic(t.CollectionID, Constants.Constants.Analytics.Layers, 1);
            _layerType.Insert(t);
        }

        public void Delete(LayerType t)
        {
            cam.UpdateAnalytic(t.CollectionID, Constants.Constants.Analytics.Layers, -1);
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
