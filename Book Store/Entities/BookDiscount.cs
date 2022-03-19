using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Entities
{
    public partial class BookDiscount
    {
        public int BookId { get; set; }
        public int DiscountId { get; set; }

        public virtual Book Book { get; set; }
        public virtual Discount Discount { get; set; }
    }
}
