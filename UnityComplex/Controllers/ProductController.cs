using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using UnityComplex.Models;
using UnityComplex.ViewModels;
using UnityComplex.ViewComponents;
using UnityComplex.DAO;
using UnityComplex.Services;
using System.Linq;
using System;

namespace UnityComplex.Controllers
{
    public class ProductController : Controller
    {
        private readonly UnityteamContext db;
        private readonly IVnPayService _vnPayService;



        public ProductController(UnityteamContext context, IVnPayService vnPayService)
        {
            db = context;
            _vnPayService = vnPayService;
        }

        //=====================  lấy danh sách sân ========================
        public IActionResult ProductList(int? idsport, int? idUser)
        {

            ProductDAO proDao = new ProductDAO(db);
            var result = proDao.SelectAllField(idsport);

            return View(result);
        }

        //  =======================  booking field ===========================

        public IActionResult ProductDetail(int idField, int idUser)
        {
            //          select sân đã chọn 
            ProductDAO proDao = new ProductDAO(db);
            var result = proDao.SelectField(idField, idUser);

            // khung giờ thuê
            var times = db.Times.ToList();

            //      lọc 7 ngày tiếp theo để đặt lịch  

            DateTime today = DateTime.Today;
            DateTime dateOnly = today.Date;
            List<FutureDate> futureDays = new List<FutureDate>();
            for (int i = 0; i < 7; i++)
            {
                futureDays.Add(new FutureDate { Dates = dateOnly.AddDays(i).Date });
            }

            //          list h đã đặt (-1 là không xét điều kiện người dùng)
            var booking = proDao.SelectAllBooked(today, idField, -1);

            //    list sản phẩm service
            var service = db.Services.ToList();

            //================================ Cường==========
            //================================ Đánh giá theo từng booking =============================

            var result1 = (from b in db.Bookings
                           join r in db.Ratings on b.IdBooking equals r.IdBooking
                           join u in db.Users on r.UserId equals u.UserId
                           where b.IdField == idField
                           select new RatingAndNameUser
                           {
                               Booking = b,
                               Rating = r,
                               User = u
                           }).ToList();



            //============================================
            // truyền vào model
            var fieldTimes = new BookingField
            {
                FieldSportVM = result,
                Time = times,
                FutureDate = futureDays,
                Bookings = booking,
                Services = service,
                SelectTime = today,
                //====== rating =========
                Object = result1
                //========
            };

            return View(fieldTimes);
        }

        //                       Lưu booked sân
        [HttpPost]
        public IActionResult ProductBooking(OrderProduct? model)
        {
            ProductDAO proDao = new ProductDAO(db);
            UserDAO userDAO = new UserDAO(db);
            Guid guid = Guid.NewGuid();

            double totalPrice = model.TotalPrice * 0.3;

            //   Create booking
            if (model.Slot != null)
            {
                if (model.myCheckboxBook == "on")
                {
                    totalPrice = model.TotalPrice;
                }

                // tạo order sân
                int id_Book = proDao.CreateBookField(model.IdField, model.IdUser, model.TotalPrice, model.SelectedDate, model.Slot, model.myCheckboxBook);
                int id_Role = userDAO.SelectRoleUser(model.IdUser);

                // Create service 
                if (model.ProId != null)
                {
                    ServiceDao serDao = new ServiceDao(db);
                    serDao.CreateBookService(model.ProId, id_Book, model.Quantity);
                }

                // tạo orderid mới
                string newOrserId = $"{id_Book}_{guid}";

                //    nếu là admin thì ko cần cọc
                if (id_Role != 1)
                {
                    // chuyển đến thanh toán
                    var vnPayModel = new VnPaymentRequestModel
                    {
                        Amount = totalPrice,
                        CreatDate = DateTime.Now,
                        Description = "thanh toans",
                        FullName = "Hieu",
                        OrderId = newOrserId,
                        checkPay = 1
                    };
                    return Redirect(_vnPayService.CreateRequestUrl(HttpContext, vnPayModel));
                }

                //      chủ sân đặt sân
                TempData["Mess"] = "Success";
                return RedirectToAction("DisplayListBooking", new { idUser = model.IdUser });
            }
            return RedirectToAction("PaymentFail");
        }

        // ========================  response VnPay  ==============================

        /*    public IActionResult PaymentFail()
            {
                return View();
            }
            public IActionResult PaymentSuccess()
            {
                return View("PaymentSuccess");
            }*/

        public IActionResult PaymentCallBack()
        {
            ProductDAO proDAO = new ProductDAO(db);
            PaymentDAO paymentDAO = new PaymentDAO(db);
            var reponse = _vnPayService.PaymentExecute(Request.Query);
            int idBooking = -1;

            // lấy idBooking
            int underscoreIndex = reponse.OrderId.IndexOf('_');


            if (underscoreIndex != -1)
            {
                idBooking = int.Parse(reponse.OrderId.Substring(0, underscoreIndex));

            }
            if (idBooking == -1)
            {
                return NotFound();
            }
            else
            {

                var infoBooking = db.Bookings.Where(id => id.IdBooking == idBooking).FirstOrDefault();
               
                //     trả về thất bại và xóa booking khỏi database
                if (reponse == null || reponse.VnPayResponseCode != "00")
                {

                    proDAO.DeleteBooking(int.Parse(reponse.OrderId.Substring(0, underscoreIndex)));

                    TempData["MessFail"] = "Fail";
                    return RedirectToAction("ProductList", new { idUser = HttpContext.Session.GetInt32("IDSession") });
                }

                //       trả về thành công và lưu vào database
                double? price = infoBooking.TotalPrice;

                if (infoBooking.Status != 0)
                {
                    price = infoBooking.TotalPrice * 0.3;
                }

                var payment = new Payment
                {
                    Deposit = (decimal)price,
                    IdField = infoBooking.IdField,
                    IdBooking = infoBooking.IdBooking,
                    PaymentTime = DateTime.Now,
                    IdUser = infoBooking.UserId
                };
                paymentDAO.Add(payment);

                TempData["Mess"] = "Success";
                return RedirectToAction("DisplayListBooking", new { idUser = infoBooking.UserId });
            }
        }

        // =====================  xử lý ajax trả về =================
        public IActionResult CheckTime(DateTime? selectedValue, int? idField)
        {
            ProductDAO proDao = new ProductDAO(db);

            var booking = proDao.SelectAllBooked(selectedValue, idField, -1);
            var times = db.Times.ToList();

            DateTime today = DateTime.Today;

            bool checkTime = selectedValue == today;

            var booked = new TimeAndBooked
            {
                Time = times,
                Bookings = booking,
                CheckTime = checkTime
            };
            return Json(booked);
        }

        //                  Hiển thị danh sách đã book
        public IActionResult DisplayListBooking(int idUser, int? idS)
        {

            ProductDAO productDAO = new ProductDAO(db);
            PaymentDAO paymentDAO = new PaymentDAO(db);
            UserDAO userDAO = new UserDAO(db);

            int idRole = userDAO.SelectRoleUser(idUser);

            var infoListBooking = productDAO.SelectAllBooked(null, -1, idUser);
            var infoListPayment = paymentDAO.SelectAllPaymentOfUser(1, idUser);
            var imageField = productDAO.SelectAllField(idS);
            var result = new BookingField
            {
                Bookings = infoListBooking,
                FieldVM = imageField,
                Payment = infoListPayment,
                Role = idRole
            };

            return View(result);
        }

        // =========================  Cancel Booking  =============================

        public IActionResult CancelBooking(CancelRequestModel model)
        {
            ProductDAO productDAO = new ProductDAO(db);
            PaymentDAO paymentDAO = new PaymentDAO(db);
            UserDAO userDAO = new UserDAO(db);
            var inforRefund = new Refund
            {
                UserId = model.idUser,
                BankName = model.BankName,
                NameOfBeni = model.NameAcc,
                AccNumber = model.accountNumber,
                IdPayment = model.idPayment,
                NumberMoney = model.refundAmount

            };
            paymentDAO.AddRefund(inforRefund);
            int? idUser = inforRefund.UserId;
            bool result = productDAO.CancelOrUpdateBooked(0, model.orderId);
            TempData["Mess"] = "Success";
            return RedirectToAction("DisplayListBooking", new { idUser = idUser });

        }

        // ==========================  xử lí refun bằng tay rồi cập nhật ==================

        public IActionResult RefundBook(int idRefund , int idUser)
        {

            ProductDAO productDAO = new ProductDAO(db);
            int? idPayment = db.Refunds.Where(refund => refund.IdRefund == idRefund).Select(refund => refund.IdPayment).FirstOrDefault();
            int? idBooking = db.Payments.Where(payment => payment.IdPayment == idPayment).Select(payment => payment.IdBooking).FirstOrDefault();
            bool check = productDAO.CancelOrUpdateBooked(1, idBooking);
            TempData["Mess"] = "Success";
            return RedirectToAction("DisplayListRefund", "Order", new { idUser = idUser });
        }







        //===============================================================
        public IActionResult FilterByPrice(int? minprice, int? maxprice)

        {
            var fields = db.Fields.AsQueryable();
            if (minprice.HasValue && maxprice.HasValue)
            {
                fields = fields.Where(p => p.MorningPrice >= minprice.Value && p.MorningPrice <= maxprice.Value);
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
            });

            return View("ProductList", result);
        }


        public IActionResult SortAsc()

        {
            var fields = db.Fields.AsQueryable();

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
            }).OrderBy(i => i.MorningPrice);

            return View("ProductList", result);
        }

        public IActionResult SortDes()

        {
            var fields = db.Fields.AsQueryable();

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
            }).OrderByDescending(i => i.MorningPrice);

            return View("ProductList", result);
        }


    }
}