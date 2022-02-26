using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=NGNCNB842;database=NFTCollectionMakerDB;integrated security=true;");
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CollectionAnalytic> CollectionAnalytics { get; set; }
        public DbSet<CollectionLayer> CollectionLayers { get; set; }
        public DbSet<ArtworkLayer> ArtworkLayers { get; set; }
        public DbSet<ArtworkTag> ArtworkTags { get; set; }
        public DbSet<LayerTag> LayerTags { get; set; }
        public DbSet<LayerType> LayerTypes { get; set; }
    }
}
