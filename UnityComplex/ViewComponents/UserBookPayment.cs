using UnityComplex.Models;

namespace UnityComplex.ViewComponents
{
    public class UserBookPayment
    {
        public User user { get; set; }
        public Booking Booking { get; set; }
        public Payment Payment { get; set; }
        public int? price { get; set; }

        public Rating? Rating { get; set; }
    }
}
