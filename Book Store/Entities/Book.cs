using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Entities
{
    public partial class Book
    {
        public Book()
        {
            BookDiscounts = new HashSet<BookDiscount>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int? PublisherId { get; set; }
        public int Pages { get; set; }
        public int? GenreId { get; set; }
        public DateTime YearPublishing { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public int? ImageId { get; set; }


        public virtual Author Author { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual DecommissionedBook DecommissionedBook { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<BookDiscount> BookDiscounts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
