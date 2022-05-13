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
    public class ArtworkManager : IArtworkService
    {
        IArtworkDal _artworkDal;
        CollectionManager cm = new CollectionManager(new EfCollectionRepository());

        public ArtworkManager(IArtworkDal artworkDal)
        {
            _artworkDal = artworkDal;
        }

        public void Add(Artwork t)
        {
            _artworkDal.Insert(t);
        }

        public int AddWithReturn(Artwork t)
        {
            _artworkDal.Insert(t);
            //Get Last index
            return int.Parse(GetList().OrderByDescending(p => p.ArtworkID)
                        .Select(r => r.ArtworkID)
                        .First().ToString());
        }

        public void Delete(Artwork t)
        {
            _artworkDal.Delete(t);
        }

        public List<Artwork> GetByCollectionID(int id)
        {
            return _artworkDal.List(x => x.CollectionID == id);
        }

        public Artwork GetByID(int id)
        {
            return _artworkDal.GetByID(id);
        }

        public List<Artwork> GetArtworkssOfUser(int userID)
        {
            List<Artwork> artworks = GetList();
            List<Artwork> userArtworks = new List<Artwork>();
            foreach (var item in artworks)
            {
                if (cm.GetByID(item.CollectionID).UserId == userID)
                {
                    userArtworks.Add(item);
                }
            }
            return userArtworks;
        }

        public int GetLastID(int collectionID)
        {
            int id = 0;
            List<Artwork> artworks = GetByCollectionID(collectionID);
            foreach (Artwork artwork in artworks)
            {
                if (artwork.ArtworkID > id)
                    id = artwork.ArtworkID;
            }
            return id;
        }

        public List<Artwork> GetList()
        {
            return _artworkDal.GetListAll();
        }

        public void Update(Artwork t)
        {
            _artworkDal.Update(t);
        }
    }
}
