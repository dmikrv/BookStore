using System;
using System.Collections.Generic;

#nullable disable

namespace Book_Store.Entities
{
    public partial class Account
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
