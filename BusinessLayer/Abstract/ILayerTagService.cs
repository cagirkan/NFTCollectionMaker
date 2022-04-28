using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ILayerTagService : IGenericService<LayerTag>
    {
        int GetTagIDofCollection(int collectiomLayerID);
    }
}
