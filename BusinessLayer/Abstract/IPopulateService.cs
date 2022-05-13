using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IPopulateService
    {
        public Task<List<Artwork>> PopulateCollection(int collectionID);
        public Task<string> GetTag(string image);
        public bool DeleteAllArtworks(int collectionID);
    }
}
