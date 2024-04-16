using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnityComplex.Models;
using UnityComplex.ViewModels;

namespace UnityComplex.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UnityteamContext db;
        public ProfileController(UnityteamContext context) => db = context;
        public IActionResult ViewProfile(int idUser)
        {
            var users = db.Users.AsQueryable();
            var user = users.FirstOrDefault(i => i.UserId == idUser);
            var result = new ProfileVM()
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile(int idUser)
        {
            var user = await db.Users.FirstOrDefaultAsync(i => i.UserId == idUser);
            if (user == null)
            {
                return NotFound();
            }

            var editProfileVM = new EditProfileVM
            {
                idUser = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View(editProfileVM);
        }

        [HttpPost]
        public IActionResult EditProfile(EditProfileVM result)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(i => i.UserId == result.idUser);

                if (user != null)
                {
                    user.FullName = result.FullName;
                    user.Email = result.Email;
                    user.PhoneNumber = result.PhoneNumber;
                    user.Address = result.Address;

                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Thay đổi thông tin thành công!";
                }

                return RedirectToAction("ViewProfile", new { idUser = result.idUser });
            }
            else
            {
                return View(result);
            }
        }
    }
}
