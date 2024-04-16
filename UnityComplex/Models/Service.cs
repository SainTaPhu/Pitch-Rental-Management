using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Service
    {
        public Service()
        {
            BookingServices = new HashSet<BookingService>();
        }

        public int IdService { get; set; }
        public string? ServiceName { get; set; }
        public int? Price { get; set; }
        public string? Decription { get; set; }
        public int? Inventory { get; set; }
        public int? IdCategory { get; set; }

        public virtual ServiceCategory? IdCategoryNavigation { get; set; }
        public virtual ICollection<BookingService> BookingServices { get; set; }
    }
}
