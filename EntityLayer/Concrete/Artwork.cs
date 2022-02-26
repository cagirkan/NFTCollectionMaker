using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    class Artwork
    {
        [Key]
        public int ArtworkID { get; set; }
        [StringLength(50)]
        public string ArtworkName { get; set; }
        [StringLength(512)]
        public string ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
