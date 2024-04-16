using Microsoft.EntityFrameworkCore;
using UnityComplex.Models;
using UnityComplex.ViewModels;

namespace UnityComplex.DAO
{
    public class ProductDAO
    {
        private readonly UnityteamContext db;

        public ProductDAO() { }
        public ProductDAO(UnityteamContext context)
        {
            db = context;
        }


        public int CreateBookField(int id_field, int id_user, int totalPrice, DateTime date, TimeSpan staTime, string? check)
        {
            try
            {
                int id_booking = 0;
                int? priceField = 0;
                int status = 1;
                if (check == "on")
                {
                    status = 0;
                }
                TimeSpan endTime = staTime.Add(TimeSpan.FromHours(1));

                var field = db.Fields.FirstOrDefault(f => f.IdField == id_field);

                if (staTime < TimeSpan.Parse("12:00:00"))
                {
                    priceField = field.MorningPrice;
                }
                else
                {
                    priceField = field.AfternoonPrice;
                }
                Booking booking = new Booking
                {
                    IdField = id_field,
                    UserId = id_user,
                    Date = date,
                    StartTime = staTime,
                    EndTime = endTime,
                    Status = status,
                    TotalPrice = totalPrice
                };
                db.Bookings.Add(booking);
                db.SaveChanges();
                id_booking = booking.IdBooking;

                return id_booking;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return -1;
            }
            return -1;
        }

        public List<FieldSportVM> SelectField(int? idF, int idUser)
        {

            try
            {
                var fields = db.Fields.AsQueryable();
                fields = fields.Where(fields => fields.IdField == idF);
                var result = fields.Select(p => new FieldSportVM
                {
                    IdField = p.IdField,
                    FieldName = p.FieldName,
                    AfternoonPrice = p.AfternoonPrice,
                    MorningPrice = p.MorningPrice,
                    Decription = p.Decription,
                    Images = db.Images
                   .Where(i => i.IdField == p.IdField)
                   .Select(i => i.Image1)
                   .ToList(),
                    SportName = db.Sports.Where(i => i.IdSport == p.IdSport).Select(i => i.SportName).FirstOrDefault(),
                    IdUser = idUser

                }).ToList();

                return result;

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return null;

            }





            /*  var fields = db.Fields.Where(id => id.IdField == idF).FirstOrDefault();*/
            return null;
        }

        public List<FieldVM> SelectAllField(int? idS)
        {
            try
            {
                var fields = db.Fields.AsQueryable();
                if (idS != null)
                {
                    fields = fields.Where(i => i.IdSport == idS);
                }
                var result = fields.Select(p => new FieldVM
                {
                    IdField = p.IdField,
                    FieldName = p.FieldName,
                    AfternoonPrice = p.AfternoonPrice,
                    MorningPrice = p.MorningPrice,
                    Decription = p.Decription,
                    Images = db.Images
                        .Where(i => i.IdField == p.IdField)
                        .Select(i => i.Image1)
                        .ToList()
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return null;
            }
            return null;
        }

        public List<Booking> SelectAllBooked(DateTime? today, int? idF, int idUser)
        {
            try
            {
                var orderedTime = db.Bookings.AsQueryable();
                if (idUser == -1)
                {
                    orderedTime = orderedTime.Where(i => i.Date == today && i.IdField == idF && (i.Status == 0 || i.Status == 1));
                }
                else
                {
                    orderedTime = orderedTime.Where(i => i.UserId == idUser).OrderByDescending(x => x.IdBooking);
                }
                var booking = orderedTime.Select(i => new Booking
                {
                    IdBooking = i.IdBooking,
                    Date = i.Date,
                    StartTime = i.StartTime,
                    EndTime = i.EndTime,
                    UserId = i.UserId,
                    IdField = i.IdField,
                    Status = i.Status,
                    TotalPrice = i.TotalPrice
                }).ToList();

                return booking;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            return null;
        }

        public void DeleteBooking(int idOr)
        {
            try
            {
                //   var bookSer = db.BookingServices.AsQueryable();
                var bookSer = db.BookingServices.AsQueryable().Where(i => i.IdBooking == idOr).ToList();
                db.BookingServices.RemoveRange(bookSer);
                db.SaveChanges();

                var book = db.Bookings.Find(idOr);
                if (book != null)
                {
                    db.Bookings.Remove(book);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public Booking selectBooking(int idB)
        {
            try
            {
                var result = db.Bookings.Find(idB);
                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return null;
        }

        public Field SelectOneField(int? idF)
        {
            try
            {
                var result = db.Fields.Find(idF);
                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return null;
        }
        public bool CancelOrUpdateBooked(int check, int? idBooked)
        {
            try
            {
                var checkBook = db.Bookings.Find(idBooked);
                if (check == 0)
                {
                    if (checkBook != null)
                    {
                        checkBook.Status = 4;
                        db.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    if (checkBook != null)
                    {
                        checkBook.Status = 3;
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return false;
        }




    }
}
