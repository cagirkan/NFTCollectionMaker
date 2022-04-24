using BusinessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class PopulateManager : IPopulateService
    {
        private readonly IConfiguration _config;
        CollectionLayerManager clm = new CollectionLayerManager(new EfCollectionLayerRepository());
        CollectionManager cm = new CollectionManager(new EfCollectionRepository());
        ArtworkManager am = new ArtworkManager(new EfArtworkRepository());
        LayerTypeManager ltm = new LayerTypeManager(new EfLayerTypeRepository());

        public PopulateManager(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> PopulateCollection(int collectionID)
        {
            Collection collection = cm.GetByID(collectionID);
            List<CollectionLayer> collectionLayers = clm.GetLayersOfCollection(collectionID);
            List<LayerType> layerTypes = ltm.GetByCollectionID(collectionID);
            string message = "Collection populated with {} artworks!";
            string mergedImagePath = "";
            StringBuilder sb = new StringBuilder();

            List<List<string>> imagePaths = clm.GetLayerPaths(collectionLayers);
            List<List<Bitmap>> layers = CreateBitmap(imagePaths);
            Bitmap bitmap = new Bitmap(layers[0][0]);
            MergeLayers(bitmap, layers[0], 0, layers, collection);

            //foreach (var item in collectionLayers)
            //{
            //    //select collectionlayers ?
            //    mergedImagePath = MergeLayers(layerImages[0], layerImages[1], collection);
            //    Artwork artwork = new Artwork();
            //    artwork.ArtworkName = mergedImagePath.Substring(mergedImagePath.LastIndexOf("/"), mergedImagePath.Length - 3);
            //    artwork.ImageURL = mergedImagePath;
            //    artwork.CollectionID = collectionID;
            //    artwork.CreatedAt = DateTime.Now;
            //    artwork.UpdatedAt = DateTime.Now;
            //    am.Add(artwork);
            //    item.Popularity++;
            //}

            sb.AppendFormat(message, "1");
            return sb.ToString();
        }

        private List<List<Bitmap>> CreateBitmap(List<List<string>> imagePaths)
        {
            List<List<Bitmap>> layers = new List<List<Bitmap>>();
            foreach (var list in imagePaths)
            {
                List<Bitmap> bitmap = new List<Bitmap>();
                foreach (var path in list)
                {
                    bitmap.Add(new Bitmap(path));
                }
                layers.Add(bitmap);
            }
            return layers;
        }

        private Bitmap MergeLayers(Bitmap artworkTemp, List<Bitmap> currentLayer, int layerIndex, List<List<Bitmap>> layers, Collection collection)
        {
            Bitmap nextArtworkTemp = artworkTemp;
            foreach (Bitmap layer in currentLayer)
            {
                var bitmap = new Bitmap(nextArtworkTemp);

                bitmap.MakeTransparent();

                using (var canvas = Graphics.FromImage(bitmap))
                {
                    canvas.DrawImage(layer, new Point());
                    canvas.Save();
                }
                if (layerIndex == layers.Count - 1)
                {
                    List<string> savePaths = GetArtworkPath(collection);
                    bitmap.Save(savePaths[0], ImageFormat.Png);
                    bitmap.Dispose();
                    Artwork artwork = new Artwork();
                    artwork.ArtworkName = savePaths[0].Substring(savePaths[0].LastIndexOf("\\") + 1);
                    artwork.CollectionID = collection.CollectionID;
                    artwork.CreatedAt = DateTime.Now;
                    artwork.UpdatedAt = DateTime.Now;
                    artwork.ImageURL = savePaths[1];
                    am.Add(artwork);
                    break;
                }
                nextArtworkTemp = MergeLayers(bitmap, layers.ElementAt(layerIndex + 1), layerIndex + 1, layers, collection);

            }

            return nextArtworkTemp;
        }

        private List<string> GetArtworkPath(Collection collection)
        {
            List<string> paths = new List<string>();
            List<Artwork> artworksOfCollection = am.GetByCollectionID(collection.CollectionID);
            var folderName = Path.Combine("Resources", "Images", "Artworks", "col" + collection.CollectionID.ToString());
            var fileName = collection.CollectionName.Replace(" ", "_") + "_" + am.GetLastID(collection.CollectionID).ToString() + ".png";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(folderPath, fileName);
            Directory.CreateDirectory(folderPath);
            var publicPath = Path.Combine("img", folderName, fileName);
            var imagePath = Path.Combine("https://localhost:44386", publicPath);
            paths.Add(fullPath);
            paths.Add(imagePath);
            return paths;
        }

        private string[] GetImagePath(int collectionID)
        {
            var folderName = Path.Combine("Resources", "Images", "CollectionLayers", "col" + collectionID.ToString());
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
                    if (apiResponse.Length > 150)
                        return "undefined";
                }
            }
            return apiResponse;
        }
    }
}
