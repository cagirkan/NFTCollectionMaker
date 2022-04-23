using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class ArtworkManager : IArtworkService
    {
        IArtworkDal _artworkDal;

        public ArtworkManager(IArtworkDal artworkDal)
        {
            _artworkDal = artworkDal;
        }

        public void Add(Artwork t)
        {
            _artworkDal.Insert(t);
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
