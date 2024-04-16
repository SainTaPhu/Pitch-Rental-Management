using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Rating
    {
        public int IdRating { get; set; }
        public int? UserId { get; set; }
        public int? IdField { get; set; }
        public int? RatingValue { get; set; }
        public TimeSpan? Time { get; set; }
        public string? Comment { get; set; }
        public int? IdBooking { get; set; }

        public virtual Booking? IdBookingNavigation { get; set; }
    }
}
