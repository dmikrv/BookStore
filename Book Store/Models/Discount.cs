using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Models
{
    public partial class Discount
    {
        public Discount()
        {
            BookDiscounts = new HashSet<BookDiscount>();
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Percent { get; set; }

        public virtual ICollection<BookDiscount> BookDiscounts { get; set; }

        public override string ToString()
        {
            return $"{StartDate.ToShortDateString()}-{EndDate.ToShortDateString()}-{Percent}";
        }
    }
}
