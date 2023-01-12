using Microsoft.AspNetCore.Mvc;

namespace MVC_BlogProject.Areas.Administrator.Controllers
{
	[Area("Administrator")]

	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
