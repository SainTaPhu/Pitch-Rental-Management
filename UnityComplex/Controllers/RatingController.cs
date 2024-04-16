using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnityComplex.Models;

namespace UnityComplex.Controllers
{
    public class RatingController : Controller
    {
        private readonly UnityteamContext db;
        public RatingController(UnityteamContext context) => db = context;

        //Create Rating
        [Route("Rating/CreateRating")]
        [HttpGet]
        public IActionResult CreateRating(int idBooking, int idPayment, int idField, int idUser)
        {
            // Truyền dữ liệu cần thiết sang trang đánh giá sân
            ViewData["IdBooking"] = idBooking;
            ViewData["IdPayment"] = idPayment;
            ViewData["IdField"] = idField;
            ViewData["IdUser"] = idUser;
            return View();
        }

        [Route("Rating/CreateRating")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRating(int idBooking, int idField, int idUser, int ratingValue, string comment)
        {
            try
            {
                // Thời gian thực đánh giá
                var currentTime = DateTime.Now.TimeOfDay;

                var rating = new Rating
                {
                    IdField = idField,
                    UserId = idUser,
                    RatingValue = ratingValue,
                    Comment = comment,
                    Time = currentTime,
                    //Status = 1,
                    IdBooking = idBooking,
                };

                db.Ratings.Add(rating);
                db.SaveChanges();
                TempData["Mess"] = "Đánh Giá Thành Công";

                return RedirectToAction("ProductList", "Product");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ProductList", "Product");
            }
        }

        //Update Rating
        [Route("Rating/UpdateRating")]
        [HttpGet]
        public IActionResult UpdateRating(int idRating)
        {
            try
            {
                // Tìm đánh giá 
                var rating = db.Ratings.FirstOrDefault(r => r.IdRating == idRating);

                if (rating != null)
                {
                    return View(rating);
                }
                else
                {
                    return RedirectToAction("ProductList", "Product");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ProductList", "Product");
            }
        }


        [Route("Rating/UpdateRating")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRating(int ratingId, int ratingValue, string comment)
        {
            try
            {
                var currentTime = DateTime.Now.TimeOfDay;
                var rating = db.Ratings.FirstOrDefault(r => r.IdRating == ratingId);

                if (rating != null)
                {
                    // Cập nhật đánh giá
                    rating.RatingValue = ratingValue;
                    rating.Comment = comment;
                    rating.Time = currentTime;

                    db.SaveChanges();
                    TempData["Mess"] = "Sửa Đánh Giá Thành Công";
                    return RedirectToAction("ProductList", "Product");
                }
                else
                {
                    return RedirectToAction("ProductList", "Product");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ProductList", "Product");
            }
        }
    }
}