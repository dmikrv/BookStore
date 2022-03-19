using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Entities
{
    public partial class Order
    {
        public int BookId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Book Book { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
