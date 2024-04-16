using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class BookingService
    {
        public int IdService { get; set; }
        public int IdBooking { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }

        public virtual Booking IdBookingNavigation { get; set; } = null!;
        public virtual Service IdServiceNavigation { get; set; } = null!;
    }
}
