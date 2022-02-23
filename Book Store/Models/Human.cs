using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Models
{
    public partial class Human
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public virtual Author Author { get; set; }
        public virtual Customer Customer { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
