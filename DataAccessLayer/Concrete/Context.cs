using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(ContextSettings.ConnectionString);
        }
        public static void EnsureCreated(IServiceProvider provider)
        {
            var context = provider.GetService<Context>();
            context.Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(model => model.GetForeignKeys()))
            //{
            //    //Disabling Delete Cascade on Each Entities
            //    foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
            //}
            SetForeingKeyAndDeleteBehavior(modelBuilder);
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

        private void SetForeingKeyAndDeleteBehavior(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artwork>().HasOne(e => e.Collection).WithMany(p => p.Artworks).HasForeignKey(f => f.CollectionID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ArtworkLayer>().HasOne(e => e.CollectionLayer).WithMany(p => p.ArtworkLayers).HasForeignKey(f => f.CollectionLayerID).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ArtworkTag>().HasOne(e => e.Tag).WithMany(p => p.ArtworkTags).HasForeignKey(f => f.TagID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ArtworkTag>().HasOne(e => e.Artwork).WithMany(p => p.ArtworkTags).HasForeignKey(f => f.ArtworkID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Collection>().HasOne(e => e.User).WithMany(p => p.Collections).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CollectionAnalytic>().HasOne(e => e.Collection).WithMany(p => p.CollectionAnalytics).HasForeignKey(f => f.CollectionID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CollectionLayer>().HasOne(e => e.Collection).WithMany(p => p.CollectionLayers).HasForeignKey(f => f.CollectionID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CollectionLayer>().HasOne(e => e.LayerType).WithMany(p => p.CollectionLayers).HasForeignKey(f => f.LayerTypeID).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<LayerTag>().HasOne(e => e.Tag).WithMany(p => p.LayerTags).HasForeignKey(f => f.TagID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LayerTag>().HasOne(e => e.CollectionLayer).WithMany(p => p.LayerTags).HasForeignKey(f => f.CollectionLayerID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<LayerType>().HasOne(e => e.Collection).WithMany(p => p.LayerTypes).HasForeignKey(f => f.CollectionID).OnDelete(DeleteBehavior.Cascade);
        }
    } 
}
