using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Drawing.Text;
using System.Security.Cryptography.X509Certificates;
using UnityComplex.Models;
using UnityComplex.Models.Authentication;
using UnityComplex.ViewModels;

namespace UnityComplex.Controllers
{
    public class StatisticController : Controller

    {
        private readonly UnityteamContext db;


        public StatisticController(UnityteamContext context)
        {
            db = context;
        }

        [Authentication]
            public IActionResult ShowData()
        {
            if (HttpContext.Session.GetInt32("RoleSession") != 1)
            {
                return RedirectToAction("Login","Login");
            }
            return View();
        }


        [HttpPost]
        public List<object> GetData(string? type,string? month,string? year) 
        {
            // model chưa lấy được giữ liệu ngày tháng năm 

            int monthValue = 0;
            int yearValue = 0;
            int typeValue = 0;

            // Parse month parameter to int
            if (!string.IsNullOrEmpty(month) && int.TryParse(month, out int parsedMonth))
            {
                monthValue = parsedMonth;
            }

            // Parse year parameter to int
            if (!string.IsNullOrEmpty(year) && int.TryParse(year, out int parsedYear))
            {
                yearValue = parsedYear;
            }

            if (!string.IsNullOrEmpty(year) && int.TryParse(type, out int parsedType))
            {
                typeValue = parsedType;
            }

            List<object> data = new List<object>();


            if (typeValue == 2) {
                List<string?> labels = db.Fields.Select(p => p.FieldName).ToList();
                data.Add(labels);


                var result = db.Bookings
                .Where(b => b.Date.Value.Month == monthValue && b.Date.Value.Year == yearValue)
                .GroupBy(b => b.IdField)
                .Select(g => new
                {
                    Total_Price = g.Sum(b => b.TotalPrice)
                })
                .ToList();

                List<int?> values = result.Select(r => (int?)r.Total_Price).ToList();
                data.Add(values);
            }
            else
            {
                List<string?> labels = db.Fields.Select(p => p.FieldName).ToList();
                data.Add(labels);


                var result = db.Bookings
                .Where(b => b.Date.Value.Month == monthValue && b.Date.Value.Year == yearValue)
                .GroupBy(b => b.IdField)
                .Select(g => new
                {
                Count = g.Count() 
                })
                .ToList();

                List<int?> values = result.Select(r => (int?)r.Count).ToList();

                data.Add(values);
            }
           

           
            



            return data;
        }
            
       


    }
}
