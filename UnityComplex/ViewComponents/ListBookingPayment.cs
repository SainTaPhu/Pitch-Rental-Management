using UnityComplex.Models;
using UnityComplex.ViewModels;

namespace UnityComplex.ViewComponents
{
	public class ListBookingPayment
	{
		public List<Booking?> Bookings  { get; set; }
        public List<FieldVM> FieldVM { get; set; }
        public List<Payment?> Payment {  get; set; }
	}
}
