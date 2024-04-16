using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnityComplex.DAO;
using UnityComplex.Models;
using UnityComplex.ViewModels;

namespace UnityComplex.Controllers
{
    public class CrudFieldController : Controller
    {
        private readonly UnityteamContext db;
        public CrudFieldController(UnityteamContext context) => db = context;
        //View All Field
        public IActionResult ViewAllField(int? idsport)
        {
            var fields = db.Fields.AsQueryable();
            if (idsport.HasValue)
            {
                fields = fields.Where(p => p.IdSport == idsport.Value);
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


            return View(result);
        }
        //Add New Field
        [Route("CrudField/AddField")]
        [HttpGet]
        public IActionResult AddField()
        {
            ViewBag.IdSport = new SelectList(db.Sports.ToList(), "IdSport", "SportName");
            return View();
        }
        //Add New Field
        [Route("CrudField/AddField")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddField(Field field, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                db.Fields.Add(field);
                db.SaveChanges();
                int fieldId = field.IdField;
                string linkPath = "\\SQL\\IMAGE\\";
                foreach (var file in files)
                {
                    string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "SQL", "IMAGE", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    db.Images.Add(new Image
                    {
                        Image1 = linkPath + fileName,
                        IdField = fieldId
                    });
                }
                await db.SaveChangesAsync();
                TempData["SuccessAddFieldMessage"] = "Thêm sân thành công!";
                return RedirectToAction("ViewAllField");
            }
            return View(field);
        }
        //Delete Field

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteField(int id)
        {
            // Xóa ảnh
            var imagesToDelete = db.Images.Where(i => i.IdField == id);
            db.Images.RemoveRange(imagesToDelete);
            await db.SaveChangesAsync();

            // Xóa sân
            var field = await db.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }

            db.Fields.Remove(field);
            await db.SaveChangesAsync();
			TempData["SuccessDeleteFieldMessage"] = "Xóa sân thành công!";

			return RedirectToAction("ViewAllField");
        }


        // Update Field
        [Route("CrudField/UpdateField/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateField(int id)
        {
            var field = await db.Fields
                        .Include(f => f.Images)
                        .FirstOrDefaultAsync(f => f.IdField == id);


            ViewBag.IdSport = new SelectList(db.Sports.ToList(), "IdSport", "SportName", field.IdSport);
            return View(field);
        }
        //Update Field
        [Route("CrudField/UpdateField/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateField(int id, Field field, List<IFormFile> files)
        {
            if (id != field.IdField)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(field).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    // Xóa ảnh
                    var oldImages = db.Images.Where(i => i.IdField == id);
                    db.Images.RemoveRange(oldImages);

                    // Thêm ảnh mới
                    string linkPath = "\\SQL\\IMAGE\\";
                    foreach (var file in files)
                    {
                        string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "SQL", "IMAGE", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        db.Images.Add(new Image
                        {
                            Image1 = linkPath + fileName,
                            IdField = id
                        });
                    }

                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FieldExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
				TempData["SuccessUpdateFieldMessage"] = "Chỉnh sửa sân thành công!";
				return RedirectToAction("ViewAllField");
            }
            ViewBag.IdSport = new SelectList(db.Sports.ToList(), "IdSport", "SportName", field.IdSport);
			
            return View(field);

        }

        private bool FieldExists(int id)
        {
            return db.Fields.Any(e => e.IdField == id);
        }
    }
}
