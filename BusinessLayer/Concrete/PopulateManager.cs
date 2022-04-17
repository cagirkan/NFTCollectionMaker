using BusinessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class PopulateManager : IPopulateService
    {
        private readonly IConfiguration _config;
        CollectionLayerManager clm = new CollectionLayerManager(new EfCollectionLayerRepository());

        public PopulateManager(IConfiguration config)
        {
            _config = config;
        }

        public void PopulateCollection(int collectionID)
        {
            List<CollectionLayer> collectionLayers = clm.GetLayersOfCollection(collectionID);
            List<Bitmap> layerImages = new List<Bitmap>();
            string[] imgPaths = GetImagePath(collectionID);
            foreach (var item in imgPaths)
            {
                Bitmap bitmap = new Bitmap(item);
                layerImages.Add(bitmap);
            }
        }

        private string[] GetImagePath(int collectionID)
        {
            var folderName = Path.Combine("Resources", "Images", collectionID.ToString());
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string[] filePaths = Directory.GetFiles(fullPath);
            return filePaths;
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
