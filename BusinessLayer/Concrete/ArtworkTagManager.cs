using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class ArtworkTagManager : IArtworkTagService
    {
        IArtworkTagDal _artworkTagDal;
        CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());
        ArtworkManager am = new ArtworkManager(new EfArtworkRepository());
        TagManager tm = new TagManager(new EfTagRepository());
        public ArtworkTagManager(IArtworkTagDal artworkTagDal)
        {
            _artworkTagDal = artworkTagDal;
        }
        public void Add(ArtworkTag t)
        {
            int colID = am.GetByID(t.ArtworkID).CollectionID;
            string tag = tm.GetByID(t.TagID).TagName;
            cam.UpdateAnalytic(colID, Constants.Constants.Analytics.ArtworksWith + char.ToUpper(tag[0]) + tag.Substring(1), 1);
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
