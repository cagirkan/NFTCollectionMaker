using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IPopulateService
    {
        public Collection PopulateCollection(List<ArtworkLayer> layers);
        public Task<string> GetTag(string image);
    }
}
