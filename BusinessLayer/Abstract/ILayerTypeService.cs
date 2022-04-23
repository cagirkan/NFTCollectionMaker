using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ILayerTypeService : IGenericService<LayerType>
    {
        public List<LayerType> GetByCollectionID(int id);
    }
}
