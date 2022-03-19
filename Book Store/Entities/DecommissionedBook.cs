using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Entities
{
    public partial class DecommissionedBook
    {
        public int BookId { get; set; }

        public virtual Book Book { get; set; }
    }
}
