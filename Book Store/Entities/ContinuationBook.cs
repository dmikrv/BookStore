using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Entities
{
    public partial class ContinuationBook
    {
        public int? BookId { get; set; }
        public int PredecessorBookId { get; set; }

        public virtual Book Book { get; set; }
        public virtual Book PredecessorBook { get; set; }
    }
}
