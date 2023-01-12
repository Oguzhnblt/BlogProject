using Microsoft.AspNetCore.Mvc;

namespace MVC_BlogProject.Areas.Administrator.Controllers
{
    public class HomeController : Controller
    {
        [Area("Administrator")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
