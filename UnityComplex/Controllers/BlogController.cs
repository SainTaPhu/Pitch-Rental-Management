﻿using Microsoft.AspNetCore.Mvc;

namespace UnityComplex.Controllers
{
	public class BlogController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
