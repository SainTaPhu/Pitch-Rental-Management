using UnityComplex.Models;
using UnityComplex.ViewModels;

namespace UnityComplex.DAO
{
    public class ServiceDao
    {
        private readonly UnityteamContext db;

        public ServiceDao() { }
        public ServiceDao(UnityteamContext context)
        {
            db = context;
        }


        public void CreateBookService(List<int?> id_Service, int id_booking, List<int?> quantity)
        {

            List<int?> newQuantity = new List<int?>();
            try
            {
                foreach (int id in quantity)
                {
                    if (id > 0)
                    {
                        newQuantity.Add(id);
                    }
                }
                // kết hợp 2 list
                //     cách 1 

                /*Cart cart = new Cart();

                if (id_Service.Count == quantity.Count)
                {
                    for (int i = 0; i < id_Service.Count; i++)
                    {
                        cart.Add(new Cart
                        {
                            Id = id_Service[i],
                            Quantity = quantity[i]
                        });
                    }
                }*/
                // cách 2

                List<Tuple<int?, int?>> newcart = new List<Tuple<int?, int?>>();

                if (id_Service.Count == newQuantity.Count)
                {
                    for (int i = 0; i < id_Service.Count; i++)
                    {
                        newcart.Add(Tuple.Create(id_Service[i], newQuantity[i]));
                    }
                }

                //    lưu vào database 

                foreach (var id in newcart)
                {
                    int idSer = (int)id.Item1;
                    var service = db.Services.FirstOrDefault(s => s.IdService == idSer);
                    BookingService bookingService = new BookingService
                    {
                        IdService = (int)id.Item1,
                        IdBooking = id_booking,
                        Quantity = (int)id.Item2,
                        Price = service?.Price ?? 0
                    };
                    db.BookingServices.Add(bookingService);
                    db.SaveChanges();
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}
