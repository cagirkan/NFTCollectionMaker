using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class ArtworkTagManager : IArtworkTagService
    {
        IArtworkTagDal _artworkTagDal;
        public ArtworkTagManager(IArtworkTagDal artworkTagDal)
        {
            _artworkTagDal = artworkTagDal;
        }
        public void Add(ArtworkTag t)
        {
            _artworkTagDal.Insert(t);
        }

        public void Delete(ArtworkTag t)
        {
            _artworkTagDal.Delete(t);
        }

        public ArtworkTag GetByID(int id)
        {
            return _artworkTagDal.GetByID(id);
        }

        public List<ArtworkTag> GetList()
        {
            return _artworkTagDal.GetListAll();
        }

        public void Update(ArtworkTag t)
        {
            _artworkTagDal.Update(t);

        }
    }
}
