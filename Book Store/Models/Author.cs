using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Models
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int HumanId { get; set; }

        public virtual Human Human { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
