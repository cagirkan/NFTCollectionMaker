using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class CollectionManager : ICollectionService
    {
        ICollectionDal _collectionDal;

        public CollectionManager(ICollectionDal collectionDal)
        {
            _collectionDal = collectionDal;
        }
        public void Add(Collection t)
        {
            _collectionDal.Insert(t);
        }

        public void Delete(Collection t)
        {
            _collectionDal.Delete(t);
        }

        public Collection GetByID(int id)
        {
            return _collectionDal.GetByID(id);
        }

        public List<Collection> GetCollectionsOfUser(int userID)
        {
            return _collectionDal.List(x => x.UserId == userID);
        }

        public List<Collection> GetList()
        {
            return _collectionDal.GetListAll();
        }

        public void Update(Collection t)
        {
            _collectionDal.Update(t);
        }
    }
}
