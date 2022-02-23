using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Models
{
    public partial class ContinuationBook
    {
        public int BookId { get; set; }
        public int PredecessorId { get; set; }

        public virtual Book Book { get; set; }
        public virtual Book Predecessor { get; set; }

        public override string ToString()
        {
            return Book.ToString();
        }
    }
}
