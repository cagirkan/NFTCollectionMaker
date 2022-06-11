using BusinessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class PopulateManager : IPopulateService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly CollectionLayerManager clm = new CollectionLayerManager(new EfCollectionLayerRepository());
        readonly CollectionManager cm = new CollectionManager(new EfCollectionRepository());
        readonly ArtworkManager am = new ArtworkManager(new EfArtworkRepository());
        readonly LayerTagManager ltam = new LayerTagManager(new EfLayerTagRepository());
        readonly ArtworkLayerManager alm = new ArtworkLayerManager(new EfArtworkLayerRepository());
        readonly ArtworkTagManager atm = new ArtworkTagManager(new EfArtworkTagRepository());

        public PopulateManager(IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Artwork>> PopulateCollection(int collectionID)
        {
            Collection collection = cm.GetByID(collectionID);
            if (collection == null)
                return null;

            //Initialize the merge parameters
            List<CollectionLayer> collectionLayers = clm.GetLayersOfCollection(collectionID);
            List<List<CollectionLayer>> colLayers = clm.GetCollectionLayersByType(collectionLayers);
            Bitmap bitmap = new Bitmap(colLayers[0][0].ImagePath);
            await MergeLayers(bitmap, null, colLayers[0], 0, colLayers, collection, new List<ArtworkLayer>(), new List<ArtworkTag>());
            bitmap.Dispose();
            return am.GetByCollectionID(collectionID);
        }

        private async Task<Bitmap> MergeLayers(Bitmap artworkTemp,
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
                Bitmap layerBitmap = new Bitmap(layer.ImagePath);
                var target = new Bitmap(nextArtworkTemp.Width, nextArtworkTemp.Height, PixelFormat.Format32bppArgb);
                var graphics = Graphics.FromImage(target);
                graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear
                graphics.DrawImage(nextArtworkTemp, 0, 0);
                graphics.DrawImage(layerBitmap, 0, 0);
                layerBitmap.Dispose();
                artworkLayers.Add(new ArtworkLayer { CollectionLayerID = layer.CollectionLayerID });
                artworkTags.Add(new ArtworkTag { TagID = ltam.GetTagIDofCollection(layer.CollectionLayerID) });
                if (layerIndex == layers.Count - 1)
                {
                    List<string> savePaths = GetArtworkPath(collection);
                    target.Save(savePaths[0], ImageFormat.Png);
                    target.Dispose();
                    graphics.Dispose();
                    Artwork artwork = new Artwork();
                    artwork.ArtworkName = savePaths[0].Substring(savePaths[0].LastIndexOf("\\") + 1);
                    artwork.CollectionID = collection.CollectionID;
                    artwork.CreatedAt = DateTime.Now;
                    artwork.UpdatedAt = DateTime.Now;
                    artwork.ImageURL = savePaths[1].Replace("\\", "/").Replace("\\", "");
                    artwork.ImagePath = savePaths[0];
                    var artworkID = am.AddWithReturn(artwork);
                    foreach (var item in artworkLayers)
                    {
                        ArtworkLayer al = new ArtworkLayer();
                        al.ArtworkID = artworkID;
                        al.CollectionLayerID = item.CollectionLayerID;
                        al.CreatedAt = DateTime.Now;
                        var layertypeID = clm.GetByID(al.CollectionLayerID).LayerTypeID;
                        alm.AddAnalytic(al,collection.CollectionID, layertypeID);
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
                    await MergeLayers(target,
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
            nextArtworkTemp.Dispose();
            return previousArtworkTemp;
        }

        private List<string> GetArtworkPath(Collection collection)
        {
            List<string> paths = new List<string>();
            var folderName = Path.Combine("Resources", "Images", "Artworks", "col" + collection.CollectionID.ToString());
            var fileName = collection.CollectionName.Replace(" ", "_") + "_" + (am.GetLastID(collection.CollectionID) + 1).ToString() + ".png";
            var folderPath = Path.Combine(_webHostEnvironment.ContentRootPath,"wwwroot", folderName);
            var fullPath = Path.Combine(folderPath, fileName);
            Directory.CreateDirectory(folderPath);
            var publicPath = Path.Combine(_config.GetValue<string>("ServerUrl"), "Resources", "Images", "Artworks", "col" + collection.CollectionID.ToString(), fileName);
            paths.Add(fullPath);
            paths.Add(publicPath);
            return paths;
        }

        public async Task<string> GetTag(string image)
        {
            string apiResponse = "";
            string api_key = _config.GetValue<string>("MLServer:APIKey");
            string addr = _config.GetValue<string>("MLServer:Addr");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(addr + api_key + "&image=" + image))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse.Length > 150)
                        return "undefined";
                }
            }
            return apiResponse;
        }

        public bool DeleteAllArtworks(int collectionID)
        {
            try
            {
                List<Artwork> artworksOfCollection = am.GetByCollectionID(collectionID);
                foreach (var item in artworksOfCollection)
                {
                    if (File.Exists(item.ImagePath))
                        File.Delete(item.ImagePath);
                    am.Delete(item);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }  
        }
    }
}
