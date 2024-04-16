using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class ServiceCategory
    {
        public ServiceCategory()
        {
            Services = new HashSet<Service>();
        }

        public int IdCategory { get; set; }
        public string? CategoryName { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
