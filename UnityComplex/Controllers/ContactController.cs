using Microsoft.AspNetCore.Mvc;

namespace UnityComplex.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
