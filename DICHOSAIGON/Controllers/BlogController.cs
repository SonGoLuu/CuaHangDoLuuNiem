using Microsoft.AspNetCore.Mvc;

namespace DICHOSAIGON.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Read()
        {
            return View();
        }
    }
}
