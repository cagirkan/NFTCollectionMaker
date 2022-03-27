using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class ArtworkTag
    {
        [Key]
        public int ArtworkTagID{ get; set; }
        public int ArtworkID { get; set; }
        public Artwork Artwork { get; set; }
        public int TagID { get; set; }
        public Tag Tag { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
