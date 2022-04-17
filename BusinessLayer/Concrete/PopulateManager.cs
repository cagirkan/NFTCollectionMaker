using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class PopulateManager : IPopulateService
    {
        private readonly IConfiguration _config;

        public PopulateManager(IConfiguration config)
        {
            _config = config;
        }

        public Collection PopulateCollection(List<ArtworkLayer> layers)
        {
            foreach (var item in layers)
            {
            }
            Collection collection = new Collection();
            collection.Artworks.Add(new Artwork());
            return collection;
        }

        //Handle Errors
        public async Task<string> GetTag(string image)
        {
            string apiResponse = "";
            string api_key = "e2b7b90c-6c00-4a5c-a236-e9833b93bd28";
            string addr = "http://164.92.249.98/api/predict?api_key=";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(addr + api_key + "&image=" + image))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    //tag = JsonConvert.DeserializeObject<string>(apiResponse);
                }
            }
            return apiResponse;
        }
    }
}
