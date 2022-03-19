﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Entities
{
    public partial class Image
    {
        public Image()
        {
        }

        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }

        public virtual Book Book { get; set; }
    }
}
