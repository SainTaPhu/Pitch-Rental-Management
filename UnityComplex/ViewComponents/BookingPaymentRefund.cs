using UnityComplex.Models;

namespace UnityComplex.ViewComponents
{
    public class BookingPaymentRefund
    {
        public Booking Booking { get; set; }
        public Refund Refund { get; set; }
        public Payment Payment { get; set; }
    }
}
