using System;
using System.Collections.Generic;

namespace UnityComplex.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Payments = new HashSet<Payment>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public int IdRole { get; set; }
        public string? Address { get; set; }

        public virtual Role? IdRoleNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
