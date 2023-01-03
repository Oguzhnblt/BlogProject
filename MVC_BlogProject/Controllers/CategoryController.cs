using Microsoft.AspNetCore.Mvc;

namespace MVC_BlogProject.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
