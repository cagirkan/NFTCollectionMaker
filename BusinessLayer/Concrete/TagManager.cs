using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
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

        public void Update(Tag t)
        {
            _tagDal.Update(t);
        }
    }
}
