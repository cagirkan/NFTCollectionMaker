﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    public class ArtworkLayer
    {
        [Key]
        public int ArtworkLayerID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
