﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityLayer.Concrete
{
    class LayerTag
    {
        [Key]
        public int LayerTagID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
