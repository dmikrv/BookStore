using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Models
{
    public partial class DecommissionedBook
    {
        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public override string ToString()
        {
            return Book.ToString();
        }
    }
}
