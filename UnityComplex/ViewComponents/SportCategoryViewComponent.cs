using Microsoft.AspNetCore.Mvc;
using UnityComplex.Models;
using UnityComplex.ViewModels;

namespace UnityComplex.ViewComponents
{
	public class SportCategoryViewComponent : ViewComponent
	{
		private readonly UnityteamContext db;
		public SportCategoryViewComponent(UnityteamContext context) => db = context;

		public IViewComponentResult Invoke()
		{
			var data = db.Sports.Select(sport => new SportCategoryVM
			{
				ID = sport.IdSport,
				Name = sport.SportName,
				
			
			}).OrderBy(s => s.ID);

			return View(data); // default.cshtml
			// return View("Name of View","data")
		}


	}
}
