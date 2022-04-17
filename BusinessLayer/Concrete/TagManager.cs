using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class TagManager : ITagService
    {
        readonly ITagDal _tagDal;
        public TagManager(ITagDal tagDal)
        {
            _tagDal = tagDal;
        }

        public int AddWithReturn(Tag t)
        {
            Tag tag = _tagDal.Get(x => x.TagName == t.TagName);
            if(tag == null)
            {
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
