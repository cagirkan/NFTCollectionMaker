using BusinessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        LayerTagManager ltam = new LayerTagManager(new EfLayerTagRepository());
        ArtworkLayerManager alm = new ArtworkLayerManager(new EfArtworkLayerRepository());
        ArtworkTagManager atm = new ArtworkTagManager(new EfArtworkTagRepository());

        public PopulateManager(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> PopulateCollection(int collectionID)
        {
            Collection collection = cm.GetByID(collectionID);
            List<CollectionLayer> collectionLayers = clm.GetLayersOfCollection(collectionID);
            string message = "Collection populated with {} artworks!";
            StringBuilder sb = new StringBuilder();

            List<List<CollectionLayer>> colLayers = clm.GetCollectionLayersByType(collectionLayers);
            Bitmap bitmap = clm.CreateBitmap(colLayers[0][0]);
            List<ArtworkLayer> artworkLayers = new List<ArtworkLayer>();
            List<ArtworkTag> artworkTags = new List<ArtworkTag>();
            MergeLayers(bitmap, null, colLayers[0], 0, colLayers, collection, artworkLayers, artworkTags);

            sb.AppendFormat(message, "1");
            return sb.ToString();
        }

        private Bitmap MergeLayers(Bitmap artworkTemp,
                                   Bitmap previousArtwork,
                                   List<CollectionLayer> currentLayer,
                                   int layerIndex,
                                   List<List<CollectionLayer>> layers,
                                   Collection collection,
                                   List<ArtworkLayer> artworkLayers,
                                   List<ArtworkTag> artworkTags)
        {
            Bitmap nextArtworkTemp = artworkTemp;
            Bitmap previousArtworkTemp = previousArtwork;
            foreach (CollectionLayer layer in currentLayer)
            {
                Bitmap layerBitmap = new Bitmap(clm.CreateBitmap(layer));
                var target = new Bitmap(nextArtworkTemp.Width, nextArtworkTemp.Height, PixelFormat.Format32bppArgb);
                var graphics = Graphics.FromImage(target);
                graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear
                graphics.DrawImage(nextArtworkTemp, 0, 0);
                graphics.DrawImage(layerBitmap, 0, 0);
                artworkLayers.Add(new ArtworkLayer { CollectionLayerID = layer.CollectionLayerID });
                artworkTags.Add(new ArtworkTag { TagID = ltam.GetTagIDofCollection(layer.CollectionLayerID) });

                if (layerIndex == layers.Count - 1)
                {
                    List<string> savePaths = GetArtworkPath(collection);
                    target.Save(savePaths[0], ImageFormat.Png);
                    Artwork artwork = new Artwork();
                    artwork.ArtworkName = savePaths[0].Substring(savePaths[0].LastIndexOf("\\") + 1);
                    artwork.CollectionID = collection.CollectionID;
                    artwork.CreatedAt = DateTime.Now;
                    artwork.UpdatedAt = DateTime.Now;
                    artwork.ImageURL = savePaths[1];
                    var artworkID = am.AddWithReturn(artwork);
                    foreach (var item in artworkLayers)
                    {
                        ArtworkLayer al = new ArtworkLayer();
                        al.ArtworkID = artworkID;
                        al.CollectionLayerID = item.CollectionLayerID;
                        al.CreatedAt = DateTime.Now;
                        alm.Add(al);
                    }
                    foreach (var item in artworkTags)
                    {
                        ArtworkTag at = new ArtworkTag();
                        at.ArtworkID = artworkID;
                        at.TagID = item.TagID;
                        at.CreatedAt = DateTime.Now;
                        atm.Add(at);
                    }
                    if (artworkLayers.Count != 0) artworkLayers.RemoveAt(artworkLayers.Count - 1);
                    if (artworkTags.Count != 0) artworkTags.RemoveAt(artworkTags.Count - 1);
                }
                else
                {
                    previousArtworkTemp = nextArtworkTemp;
                    nextArtworkTemp = MergeLayers(target,
                                                  nextArtworkTemp,
                                                  layers.ElementAt(layerIndex + 1),
                                                  layerIndex + 1,
                                                  layers,
                                                  collection,
                                                  artworkLayers,
                                                  artworkTags);
                }
            }
            if (artworkLayers.Count != 0) artworkLayers.RemoveAt(artworkLayers.Count - 1);
            if (artworkTags.Count != 0) artworkTags.RemoveAt(artworkTags.Count - 1);
            return previousArtworkTemp;
        }

        private List<string> GetArtworkPath(Collection collection)
        {
            List<string> paths = new List<string>();
            List<Artwork> artworksOfCollection = am.GetByCollectionID(collection.CollectionID);
            var folderName = Path.Combine("Resources", "Images", "Artworks", "col" + collection.CollectionID.ToString());
            var fileName = collection.CollectionName.Replace(" ", "_") + "_" + (am.GetLastID(collection.CollectionID) + 1).ToString() + ".png";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(folderPath, fileName);
            Directory.CreateDirectory(folderPath);
            var publicPath = Path.Combine("img", "Artworks", "col" + collection.CollectionID.ToString(), fileName);
            var imagePath = Path.Combine("https://localhost:44386", publicPath);
            paths.Add(fullPath);
            paths.Add(imagePath);
            return paths;
        }

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
