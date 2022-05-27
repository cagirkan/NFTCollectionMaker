using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class ArtworkLayerManager : IArtworkLayerService
    {
        readonly IArtworkLayerDal _artworkLayer;

        public ArtworkLayerManager(IArtworkLayerDal artworkLayer)
        {
            _artworkLayer = artworkLayer;
        }

        public void Add(ArtworkLayer t)
        {
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
    }
}
