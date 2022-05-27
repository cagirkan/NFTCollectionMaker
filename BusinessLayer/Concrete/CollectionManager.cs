using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Concrete
{
    public class CollectionManager : ICollectionService
    {
        readonly ICollectionDal _collectionDal;
        readonly UserManager um = new UserManager(new EfUserRepository());
        readonly CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());

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
            var id = int.Parse(GetList().OrderByDescending(p => p.CollectionID)
                        .Select(r => r.CollectionID)
                        .First().ToString());
            cam.InitializeAnalytics(id);
            return id;
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
