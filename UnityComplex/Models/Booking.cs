using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Booking
    {
        public Booking()
        {
            BookingServices = new HashSet<BookingService>();
            Payments = new HashSet<Payment>();
        }

        public int IdBooking { get; set; }
        public int? UserId { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Status { get; set; }
        public int? IdField { get; set; }
        public int? TotalPrice { get; set; }

        public virtual Field? IdFieldNavigation { get; set; }
        public virtual User? User { get; set; }
        public virtual Rating? Rating { get; set; }
        public virtual ICollection<BookingService> BookingServices { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
