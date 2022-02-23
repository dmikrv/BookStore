using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int HumanId { get; set; }
        public string Phone { get; set; }

        public virtual Human Human { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
