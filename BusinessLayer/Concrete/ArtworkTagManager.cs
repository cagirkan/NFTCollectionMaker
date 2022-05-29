using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Concrete
{
    public class ArtworkTagManager : IArtworkTagService
    {
        readonly IArtworkTagDal _artworkTagDal;
        readonly CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());
        readonly ArtworkManager am = new ArtworkManager(new EfArtworkRepository());
        readonly TagManager tm = new TagManager(new EfTagRepository());
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

        public List<int> GetTagsByID(int artworkID)
        {
            var tagIDs = new List<int>();
            var tags = _artworkTagDal.List(x => x.ArtworkID == artworkID);
            foreach (var tag in tags)
            {
                tagIDs.Add(tag.TagID);
            }
            return tagIDs;
        }
    }
}
