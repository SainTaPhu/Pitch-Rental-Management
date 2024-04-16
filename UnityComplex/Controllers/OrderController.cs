using Microsoft.AspNetCore.Mvc;
using UnityComplex.Models;
using UnityComplex.DAO;
using UnityComplex.ViewComponents;
using UnityComplex.ViewModels;
using UnityComplex.Services;

namespace UnityComplex.Controllers
{
    public class OrderController : Controller
    {

        private readonly UnityteamContext db;
		private readonly IVnPayService _vnPayService;

		public OrderController(UnityteamContext context, IVnPayService vnPayService)
        {
            db = context;
            _vnPayService = vnPayService;
        }


        public IActionResult ViewHistory(int idUser)
        {
            PaymentDAO paymentDAO = new PaymentDAO(db);
            ProductDAO productDAO = new ProductDAO(db);


            var result = paymentDAO.SelectAllPaymentOfUser(0, idUser);

            return View(result);
        }
        public IActionResult ViewHistoryDetail(int? idPayment, int? idBooking)
        {
            UserDAO userDAO = new UserDAO(db);
            ProductDAO productsDAO = new ProductDAO(db);
            //  acc user
            if (idPayment != null)
            {
                var inforPayment = db.Payments.Where(p => p.IdPayment == idPayment).FirstOrDefault();
                var inforBooking = db.Bookings.Where(b => b.IdBooking == inforPayment.IdBooking).FirstOrDefault();

                var user = userDAO.SelectUser(inforPayment.IdUser);
                var inforRating = db.Ratings.Where(r => r.IdBooking == inforBooking.IdBooking).FirstOrDefault();
                var result = new UserBookPayment
                {
                    Payment = inforPayment,
                    Booking = inforBooking,
                    user = user,
                    Rating = inforRating
                };
                return View(result);

            }
            // acc admin
            if (idBooking != null)
            {
                var inforBooking = db.Bookings.Where(b => b.IdBooking == idBooking).FirstOrDefault();
                var user = userDAO.SelectUser(inforBooking.UserId);

                var result = new UserBookPayment
                {
                    Booking = inforBooking,
                    user = user
                };



                return View(result);
            }

            return NotFound();

        }

        public IActionResult CancelOrder(int idBooking, int? idPayment, int? idRole)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO(db);
                PaymentDAO paymentDAO = new PaymentDAO(db);

                var infoBooking = productDAO.selectBooking(idBooking);

                if (idRole == 1)
                {
                    productDAO.DeleteBooking(idBooking);
                    TempData["Mess"] = "Success";
                    return RedirectToAction("DisplayListBooking", "Product", new { idUser = infoBooking.UserId });
                }
                else
                {
                    var ifoPayment = paymentDAO.selectPayment(idPayment);
                    var inforField = productDAO.SelectOneField(infoBooking.IdField);
                    int? price = 0;

                    if (infoBooking.StartTime < TimeSpan.Parse("12:00:00"))
                    {
                        price = inforField.MorningPrice;
                    }
                    else
                    {
                        price = inforField.AfternoonPrice;
                    }

                    // int priceFieldRent = productDAO.SelectPriceFieldRent(infoBooking.StartTime, inforField.);
                    var result = new UserBookPayment
                    {
                        Booking = infoBooking,
                        Payment = ifoPayment,
                        price = price

                    };

                    return View(result);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
            return View();
        }

        public IActionResult PaymentRemain(int idBooking)
        {
            ProductDAO productsDAO = new ProductDAO(db);
            PaymentDAO paymentsDAO = new PaymentDAO(db);
            UserDAO userDAO = new UserDAO(db);
            Guid guid = Guid.NewGuid();
            double priceRemain = 1;
            string newOrserId = $"{idBooking}_{guid}";

            var infoBooking = productsDAO.selectBooking(idBooking);

            int roleUser = userDAO.SelectRoleUser(infoBooking.UserId);
            if (roleUser != 1)
            {
                var infoPayment = db.Payments.Where(p => p.IdBooking == idBooking).FirstOrDefault();
                priceRemain = (infoBooking.TotalPrice ?? 1.0) - (double)(infoPayment.Deposit ?? (decimal)1.0);
            }
            else
            {
                priceRemain = (double)infoBooking.TotalPrice;
            }
            var vnPayModel = new VnPaymentRequestModel
            {
                Amount = priceRemain,
                CreatDate = DateTime.Now,
                Description = "thanh toans",
                FullName = "Hieu",
                OrderId = newOrserId,
                checkPay = 0
            };
            return Redirect(_vnPayService.CreateRequestUrl(HttpContext, vnPayModel));

        }


        public IActionResult PaymentRemainCallBack()
        {

            ProductDAO productDAO = new ProductDAO(db);
            PaymentDAO paymentDAO = new PaymentDAO(db);
            UserDAO userDAO = new UserDAO(db);

            var reponse = _vnPayService.PaymentExecute(Request.Query);
            // lấy idBooking
            int underscoreIndex = reponse.OrderId.IndexOf('_');
            int idBooking = int.Parse(reponse.OrderId.Substring(0, underscoreIndex));


            //    check idBooking
            var infoBooking = db.Bookings.Find(idBooking);
            double priceRemain = 0;
            int roleUser = userDAO.SelectRoleUser(infoBooking.UserId);
            if (roleUser != 1)
            {
                var infoPayment = db.Payments.Where(p => p.IdBooking == idBooking).FirstOrDefault();
                priceRemain = (double)(infoBooking.TotalPrice ?? 1.0) - (double)(infoPayment.Deposit ?? (decimal)1.0);
            }
            else
            {
                priceRemain = (double)infoBooking.TotalPrice;
            }
            //  =====================================
            if (reponse == null || reponse.VnPayResponseCode != "00")
            {

                TempData["MessFail"] = "Fail";
                if (HttpContext.Session.GetInt32("RoleSession") == roleUser)
                {
                    return RedirectToAction("DisplayListBooking", "Product", new { idUser = infoBooking.UserId });
                }
                else
                {
                    return RedirectToAction("DisplayListRefund", new { idUser = SessionExtensions.GetInt32(HttpContext.Session, "IDSession") });
                }

            }

            var payment = new Payment
            {
                Deposit = (decimal)priceRemain,
                IdField = infoBooking.IdField,
                IdBooking = infoBooking.IdBooking,
                PaymentTime = DateTime.Now,
                IdUser = infoBooking.UserId
            };
            paymentDAO.Add(payment);
            infoBooking.Status = 0;
            db.SaveChanges();
            TempData["Mess"] = "Success";
            if (HttpContext.Session.GetInt32("RoleSession") == roleUser)
            {
                return RedirectToAction("DisplayListBooking", "Product", new { idUser = infoBooking.UserId });
            }
            else
            {
                return RedirectToAction("DisplayListRefund", new { idUser = SessionExtensions.GetInt32(HttpContext.Session, "IDSession") });
            }

        }

        public IActionResult PaymentError()
        {


            return View();
        }

        // ============================== danh sách chưa thanh toán hết và cần hoàn tiền
        public IActionResult DisplayListRefund(int idUser, int? idS)
        {
            ProductDAO productsDAO = new ProductDAO(db);
            UserDAO userDAO = new UserDAO(db);
            int id_Role = userDAO.SelectRoleUser(idUser);
            if (id_Role == 1)
            {
                var listBooking = db.Bookings.Where(b => (b.Status == 1 || b.Status == 4)).ToList();
                var listPayment = db.Payments.GroupBy(p => p.IdBooking).Select(g => g.FirstOrDefault()).ToList();
                var imageField = productsDAO.SelectAllField(idS);
                var result = new ListBookingPayment
                {
                    Bookings = listBooking,
                    Payment = listPayment,
                    FieldVM = imageField
                };

                return View(result);

            }
            return View();
        }

        public IActionResult ViewRefundDetail(int idPayment)
        {
            int? idBooking = db.Payments.Where(p => p.IdPayment == idPayment).Select(p => p.IdBooking).FirstOrDefault();
            var infoBooking = db.Bookings.Find(idBooking);
            var inforRefund = db.Refunds.Where(r => r.IdPayment == idPayment).FirstOrDefault();
            var inforPayment = db.Payments.Find(idPayment);

            var result = new BookingPaymentRefund
            {
                Booking = infoBooking,
                Refund = inforRefund,
                Payment = inforPayment
            };
            return View(result);
        }

        public IActionResult PaymentRemainCOD(int idBooking, double remain, int idPayment)
        {
            if (idBooking != null)
            {
                var inforBook = db.Bookings.Find(idBooking);
                var inforPayment = db.Payments.Find(idPayment);
                double priceDeposit = (double)inforPayment.Deposit;


                inforBook.Status = 0;
                inforPayment.Deposit = (decimal)(priceDeposit + remain);
                db.SaveChanges();
                TempData["Mess"] = "Success";
                return RedirectToAction("DisplayListRefund", new { idUser = SessionExtensions.GetInt32(HttpContext.Session, "IDSession") });
            }
            return NotFound();
        }

        public IActionResult ViewHistoryTranAdmin(int idUser)
        {

            var inforPayment = db.Payments.ToList().OrderByDescending(p => p.IdPayment);
            return View(inforPayment);
        }

        public IActionResult ViewHistoryTranAdminDetail(int? idPayment, int? idBooking)
        {
            UserDAO userDAO = new UserDAO(db);
            ProductDAO productsDAO = new ProductDAO(db);
            //  acc user
            if (idPayment != null)
            {
                var inforPayment = db.Payments.Where(p => p.IdPayment == idPayment).FirstOrDefault();
                var inforBooking = db.Bookings.Where(b => b.IdBooking == inforPayment.IdBooking).FirstOrDefault();

                var user = userDAO.SelectUser(inforPayment.IdUser);
                var result = new UserBookPayment
                {
                    Payment = inforPayment,
                    Booking = inforBooking,
                    user = user
                };
                return View(result);

            }
            // acc admin
            if (idBooking != null)
            {
                var inforBooking = db.Bookings.Where(b => b.IdBooking == idBooking).FirstOrDefault();
                var user = userDAO.SelectUser(inforBooking.UserId);

                var result = new UserBookPayment
                {
                    Booking = inforBooking,
                    user = user
                };



                return View(result);
            }

            return NotFound();


        }

    }
}
