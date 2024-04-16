using UnityComplex.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using UnityComplex.Models;
using UnityComplex.ViewModels;

namespace UnityComplex.ViewComponents
{
    public class BookingField
    {

        public List<FieldSportVM> FieldSportVM { get; set; }
        public List<Time?> Time { get; set; }

        public List<FutureDate> FutureDate { get; set; }

        public List<Booking?> Bookings { get; set; }

        public List<Service> Services { get; set; }
        public List<FieldVM> FieldVM { get; set; }

        public List<Payment> Payment { get; set; }

        public DateTime SelectTime { get; set; }

        // =========== rating ===============
        public List<RatingAndNameUser> Object { get; set; }

        //==================================
        public int Role { get; set; }
		public BookingField()
        {

            FieldSportVM = new List<FieldSportVM>();
            Time = new List<Time?>();
            FutureDate = new List<FutureDate>();
            Bookings = new List<Booking?>();
            Services = new List<Service>();

        }

    }
}
