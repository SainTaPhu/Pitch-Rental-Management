using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Sport
    {
        public Sport()
        {
            Fields = new HashSet<Field>();
        }

        public int IdSport { get; set; }
        public string? SportName { get; set; }

        public virtual ICollection<Field> Fields { get; set; }
    }
}
