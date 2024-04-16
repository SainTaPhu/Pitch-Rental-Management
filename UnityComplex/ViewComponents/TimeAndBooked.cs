using UnityComplex.Models;

namespace UnityComplex.ViewComponents
{
    public class TimeAndBooked
    {

        public List<Time?> Time { get; set; }

        public List<Booking?> Bookings { get; set; }

        public bool CheckTime { get; set; }

        public TimeAndBooked()
        {
            Time = new List<Time?>();
            Bookings = new List<Booking?>();
        }
    }
}
