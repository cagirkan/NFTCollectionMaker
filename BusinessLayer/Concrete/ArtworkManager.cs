using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Concrete
{
    public class ArtworkManager : IArtworkService
    {
        readonly IArtworkDal _artworkDal;
        readonly CollectionManager cm = new CollectionManager(new EfCollectionRepository());
        readonly CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());
        readonly CollectionLayerManager clm = new CollectionLayerManager(new EfCollectionLayerRepository());
        readonly ArtworkLayerManager alm = new ArtworkLayerManager(new EfArtworkLayerRepository());

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
            cam.UpdateAnalytic(t.CollectionID, Constants.Constants.Analytics.Artworks, 1);
            _artworkDal.Insert(t);
            //Get Last index
            return int.Parse(GetList().OrderByDescending(p => p.ArtworkID)
                        .Select(r => r.ArtworkID)
                        .First().ToString());
        }

        public void Delete(Artwork t)
        {
            List<int> collectionLayers = alm.GetLayersOfArtwork(t.ArtworkID);
            List<string> typesOfArtwork = clm.getTypesOfArtwork(collectionLayers);
            cam.UpdateAnalytic(t.CollectionID, Constants.Constants.Analytics.Artworks, -1);
            foreach (var item in typesOfArtwork)
            {
                cam.UpdateAnalytic(t.CollectionID, Constants.Constants.Analytics.ArtworksWith + char.ToUpper(item[0]) + item.Substring(1), -1);
            }
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
