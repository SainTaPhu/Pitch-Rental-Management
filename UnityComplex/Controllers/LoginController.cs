using Microsoft.AspNetCore.Mvc;
using UnityComplex.Models;
using UnityComplex.ViewModels;

using System.Net;
using System.Net.Mail;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityComplex.Controllers
{
	public class LoginController : Controller
	{
		private readonly UnityteamContext db;

		private static readonly Random random = new Random();
		public LoginController(UnityteamContext context)  {
			db = context;
			
	}
		[HttpGet]
		public IActionResult Login()
		{

			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginVM model)
		{
			if (ModelState.IsValid)
			{
				var user = db.Users.FirstOrDefault(u => u.Email.Equals(model.Email));
				if (user == null || !user.Password.Equals(model.Password))
				{
					TempData["LoginFail"] = "Email hoặc mật khẩu không chính xác";
					return View();
				}

				HttpContext.Session.SetInt32("RoleSession", (int)user.IdRole);


				HttpContext.Session.SetInt32("IDSession", user.UserId);

				return RedirectToAction("Index", "Home");
			}

			return View();
		}


		[HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public IActionResult SignUp(RegisterVM model)
		{

			if (ModelState.IsValid)
			{
				var user = db.Users.FirstOrDefault(p => p.Email == model.Email);
				if (user != null)
				{
					TempData["DuplicateEmailMessage"] = "Email đã được đăng kí cho tài khoản khác";
					return View();
				}
				else
				{
					var newuser = new User()
					{
						Email = model.Email,
						Password = model.Password,
						FullName = model.FullName,
						Address = model.Address,
						PhoneNumber = model.PhoneNumber,
						IdRole = 2
					};
					db.Add(newuser);
					db.SaveChanges();

					TempData["SignUpSuccessMessage"] = "Đăng kí thành công";
					return View();
				}


			}
			return View();
		}


		public IActionResult Logout()
		{

			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}

        [HttpGet]
        public IActionResult ChangePassword(int idUser)
        {
            var user = db.Users.FirstOrDefault(i => i.UserId == idUser);
            if (user == null)
            {
                return NotFound();
            }

            var changePassVM = new ChangePassVM
            {
                idUser = user.UserId,
                Email = user.Email
            };

            return View(changePassVM);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePassVM result)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(i => i.UserId == result.idUser);

                if (user != null)
                {
                    if (user.Password == result.CurrentPassword && result.NewPassword == result.ConfirmPassword)
                    {
                        user.Password = result.NewPassword;
                        db.SaveChanges();
                        TempData["ChangePassSuccess"] = "Bạn đã thay đổi mật khẩu thành công!";
                        return RedirectToAction("ChangePassWord", new { idUser = result.idUser });
                    }
                    else
                    {
                        TempData["ChangePassFail"] = "Mật khẩu hiện tại không chính xác hoặc mật khẩu mới không trùng khớp!";
                        return View(result);
                    }
                }
                else
                {
                    TempData["ChangePassFail"] = "Người dùng không tồn tại!";
                    return View(result);
                }
            }
            else
            {
                TempData["ChangePassFail"] = "Xác nhận mật khẩu không trùng khớp!";
                return View(result);
            }
        }
        [HttpGet] 
		public IActionResult ForgotPassWord()
		{
			return View();
		}

        [HttpPost]
        public IActionResult ForgotPassWord(OtpVM model)
        {
			if (ModelState.IsValid)

			{
				string? email = HttpContext.Session.GetString("CurrentEmail");
				string? otp = HttpContext.Session.GetString("CurrentOTP");
				var user = db.Users.FirstOrDefault(p => p.Email == email);
				

				if (model.OTP == otp)
				{
					var newPassword = GenerateRandomPassWord(8);
					user.Password = newPassword;
					db.SaveChanges();

					string fromEmail = "cuongphde170060@fpt.edu.vn";
					string fromPassword = "tqdkkkxesbfwhfah";

					MailMessage message = new MailMessage();
					message.From = new MailAddress(fromEmail);
					message.Subject = "Cập nhật mật khẩu";
					message.To.Add(new MailAddress(email));
					message.Body = $"<html><body>Mật khẩu mới của bạn là: {newPassword}</body></html>";
					message.IsBodyHtml = true;

					var smtpClient = new SmtpClient("smtp.gmail.com")
					{
						Port = 587,
						Credentials = new NetworkCredential(fromEmail, fromPassword),
						EnableSsl = true
					};

					smtpClient.Send(message);
					TempData["SendOTPSuccessMessage"] = "OTP hợp lệ,Vui lòng kiểm tra email để lấy lại mật khẩu";
					return RedirectToAction("VerifyOTP", "Login");
				}
				TempData["SendOTPFailMessage"] = "OTP không hợp lệ ";
				return RedirectToAction("VerifyOTP", "Login");
			}
			return View();
        }

        [HttpGet]
        public IActionResult VerifyOTP()
        {
            return View();
        }


        public IActionResult VerifyOTP(ForgotPassVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(p => p.Email == model.Email);


                if (user != null)
                {
                    var otp = GenerateOTP(6);

					HttpContext.Session.SetString("CurrentOTP", otp);


					HttpContext.Session.SetString("CurrentEmail", model.Email);
					string fromEmail = "cuongphde170060@fpt.edu.vn";
                    string fromPassword = "tqdkkkxesbfwhfah";

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(fromEmail);
                    message.Subject = "Verify OTP";
                    message.To.Add(new MailAddress(model.Email));
                    message.Body = $"<html><body>OTP của bạn là: {otp}</body></html>";
                    message.IsBodyHtml = true;

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(fromEmail, fromPassword),
                        EnableSsl = true
                    };

                    smtpClient.Send(message);
                    TempData["SendMailSuccessMessage"] = "Vui lòng kiểm tra email để lấy mã OTP";
					return View();
                }
                TempData["SendMailFailMessage"] = "Email chưa được đăng kí ";
                return RedirectToAction("ForgotPassWord", "Login");
            }
            return View();
        }

		public string GenerateOTP(int length)
		{
			const string numericChars = "0123456789";
			var random = new Random();
			var stringChars = new char[length];
			
			for (int i = 0; i < length; i++)
			{
				stringChars[i] = numericChars[random.Next(numericChars.Length)];
			}
			return new string(stringChars);
		}

        public string GenerateRandomPassWord(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
			const string numericChars = "0123456789";

			var random = new Random();
			var stringChars = new char[length];

		
			stringChars[0] = uppercaseChars[random.Next(uppercaseChars.Length)];

	
			stringChars[1] = lowercaseChars[random.Next(lowercaseChars.Length)];

	
			stringChars[2] = numericChars[random.Next(numericChars.Length)];


			for (int i = 3; i < length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			for (int i = 0; i < length; i++)
			{
				int rndIndex = random.Next(length);
				char temp = stringChars[i];
				stringChars[i] = stringChars[rndIndex];
				stringChars[rndIndex] = temp;
			}

			return new string(stringChars);
		}



	}
}
