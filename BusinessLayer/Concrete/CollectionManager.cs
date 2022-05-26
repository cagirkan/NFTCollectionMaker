using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class CollectionManager : ICollectionService
    {
        ICollectionDal _collectionDal;
        UserManager um = new UserManager(new EfUserRepository());


        public CollectionManager(ICollectionDal collectionDal)
        {
            _collectionDal = collectionDal;
        }
        public void Add(Collection t)
        {
            _collectionDal.Insert(t);
        }

        public int AddWithReturn(Collection t)
        {
            _collectionDal.Insert(t);
            //Get Last index
            return int.Parse(GetList().OrderByDescending(p => p.CollectionID)
                        .Select(r => r.CollectionID)
                        .First().ToString());
        }

        public void Delete(Collection t)
        {
            _collectionDal.Delete(t);
        }

        public Collection GetByID(int id)
        {
            return _collectionDal.GetByID(id);
        }

        public Collection GetCollectionsOfUser(string userName)
        {
            int userID = um.getIdByUsername(userName);
            return _collectionDal.Get(x => x.UserId == userID);
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
