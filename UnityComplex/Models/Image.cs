using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Image
    {
        public int IdImage { get; set; }
        public string? Image1 { get; set; }
        public int? IdField { get; set; }

        public virtual Field? IdFieldNavigation { get; set; }
    }
}
