using AutoMapper;
using EntityLayer.Concrete;

namespace NFTCollectionMakerAPI.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CollectionLayer, CollectionLayerViewModel>();
            CreateMap<Collection, CollectionViewModel>();
            CreateMap<Artwork, ArtworkViewModel>();
        }
    }
}
