using BlogProject.Core.Service;
using BlogProject.Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MVC_BlogProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICoreService<Category> categoryService;
        private readonly ICoreService<Post> postService;

        public CategoryController(ICoreService<Category> categoryService, ICoreService<Post> postService)
        {
            this.categoryService = categoryService;
            this.postService = postService;
        }
        public IActionResult Index()
        {
            return View(categoryService.GetActive(x=>x.Posts).ToList());
        }

        public IActionResult PostsByCategory(Guid id)
        {
            return View(postService.GetActive(x=>x.Kullanici, a=>a.Comments).Where(b=>b.CategoryID == id).ToList());    
        }
    }
}
