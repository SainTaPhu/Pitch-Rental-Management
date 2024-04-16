using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int IdRole { get; set; }
        public string? Role1 { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
