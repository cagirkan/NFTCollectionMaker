using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ILayerTypeService : IGenericService<LayerType>
    {
        public List<LayerType> GetByCollectionID(int id);
        public List<LayerType> GetLayerTypesOfUser(int userID);
        public LayerType GetByIDAuth(int id, int userID);
    }
}
