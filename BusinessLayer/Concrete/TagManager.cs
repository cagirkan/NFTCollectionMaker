using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Concrete
{
    public class TagManager : ITagService
    {
        readonly ITagDal _tagDal;
        readonly CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());
        public TagManager(ITagDal tagDal)
        {
            _tagDal = tagDal;
        }

        public int AddWithReturn(Tag t, int collectionID)
        {
            Tag tag = _tagDal.Get(x => x.TagName == t.TagName);
            if(tag == null)
            {
                CollectionAnalytic collectionAnalytic = new CollectionAnalytic();
                collectionAnalytic.CollectionID = collectionID;
                collectionAnalytic.Key = Constants.Constants.Analytics.ArtworksWith + char.ToUpper(t.TagName[0]) + t.TagName.Substring(1);
                collectionAnalytic.Value = 0;
                cam.Add(collectionAnalytic);
                _tagDal.Insert(t);
                return int.Parse(GetList().OrderByDescending(p => p.TagID)
                                .Select(r => r.TagID)
                                .First().ToString());
            }
            else
            {
                return tag.TagID;
            }
           
        }
        public void Add(Tag t)
        {
            _tagDal.Insert(t);
        }

        public void Delete(Tag t)
        {
            _tagDal.Delete(t);
        }

        public Tag GetByID(int id)
        {
            return _tagDal.GetByID(id);
        }

        public List<Tag> GetList()
        {
            return _tagDal.GetListAll();
        }

        public bool isTagNameUnique(string name)
        {
            Tag tag = _tagDal.Get(x => x.TagName.Equals(name));
            if (tag == null)
                return true;
            return false;
        }

        public void Update(Tag t)
        {
            _tagDal.Update(t);
        }
    }
}
