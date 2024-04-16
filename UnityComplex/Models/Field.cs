using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class Field
    {
        public Field()
        {
            Bookings = new HashSet<Booking>();
            Images = new HashSet<Image>();
            Payments = new HashSet<Payment>();
        }

        public int IdField { get; set; }
        public string? FieldName { get; set; }
        public string? Decription { get; set; }
        public int? MorningPrice { get; set; }
        public int? AfternoonPrice { get; set; }
        public int? IdSport { get; set; }

        public virtual Sport? IdSportNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
