using BlogProject.Core.Service;
using BlogProject.Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MVC_BlogProject.Areas.Administrator.Controllers
{
	[Area("Administrator")]

	public class HomeController : Controller
    {
		private readonly ICoreService<Category> categoryService;
		private readonly ICoreService<User> userService;
		private readonly ICoreService<Post> postService;
		private readonly ICoreService<Comment> commentService;


		public HomeController(ILogger<HomeController> logger, ICoreService<Category> categoryService, ICoreService<User> userService, ICoreService<Post> postService, ICoreService<Comment> commentService)
		{
			this.categoryService = categoryService;
			this.userService = userService;
			this.postService = postService;
			this.commentService = commentService;
		}

        public IActionResult Index()
        {
			ViewBag.User = userService.GetActive().Count;
			ViewBag.Category = categoryService.GetActive().Count;
			ViewBag.Post = postService.GetActive().Count;
			ViewBag.Comment = commentService.GetActive().Count;


			return View();
        }

    }
}
