using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Refunds = new HashSet<Refund>();
        }

        public int IdPayment { get; set; }
        public decimal? Deposit { get; set; }
        public int? IdField { get; set; }
        public int? IdBooking { get; set; }
        public DateTime? PaymentTime { get; set; }
        public int? IdUser { get; set; }

        public virtual Booking? IdBookingNavigation { get; set; }
        public virtual Field? IdFieldNavigation { get; set; }
        public virtual User? IdUserNavigation { get; set; }
        public virtual ICollection<Refund> Refunds { get; set; }
    }
}
