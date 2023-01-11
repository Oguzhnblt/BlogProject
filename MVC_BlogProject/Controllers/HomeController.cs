using BlogProject.Core.Entity.Enum;
using BlogProject.Core.Service;
using BlogProject.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using MVC_BlogProject.Models;
using MVC_BlogProject.Models.ViewModels;
using System.Diagnostics;

namespace MVC_BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICoreService<Category> categoryService;
        private readonly ICoreService<User> userService;
        private readonly ICoreService<Post> postService;
        private readonly ICoreService<Comment> commentService;


        public HomeController(ILogger<HomeController> logger, ICoreService<Category> categoryService, ICoreService<User> userService, ICoreService<Post> postService, ICoreService<Comment> commentService)
        {
            _logger = logger;
            this.categoryService = categoryService;
            this.userService = userService;
            this.postService = postService;
            this.commentService = commentService;
        }

        public IActionResult Index()
        {
            // Aktif olan postları döndür

            return View(postService.GetActive());
        }

        public IActionResult PostsByCategoryID(Guid id) // Kategorinin ID'si
        {
            // Kategori ID'ye göre aktif postları döndür.

            return View(postService.GetDefault(x => x.CategoryID == id));
        }

        public IActionResult Post(Guid id) // Gönderinin ID'si
        {
            // Post'u göstermek için. Gösterirken de ViewCount sayısını 1 arttıracağız

            Post okunanPost = postService.GetById(id);
            okunanPost.ViewCount++;
            postService.Update(okunanPost);

            PostDetailVM vm = new PostDetailVM();
            vm.Post = okunanPost;
            vm.Category = categoryService.GetById(okunanPost.CategoryID);
            vm.User = userService.GetById(okunanPost.UserID);
            vm.Comments = commentService.GetDefault(x => x.Status == Status.Active && x.PostID == okunanPost.ID);


            // Random RelatedPost döndürmek için

            Random r = new Random(); // Random nesnesi oluşturduk.
            vm.Categories = categoryService.GetActive();

            for (int i = 0; i < 3; i++)
            {
                vm.RelatedPost.Add(postService.GetActive().ElementAt(r.Next(0, postService.GetActive().Count)));

                // Okunan kategorinin içerisindeki rastgele bir postu bulmak için
                //vm.RelatedPost.Add(postService.GetDefault(x=>x.CategoryID == okunanPost.CategoryID).ElementAt(r.Next(0, postService.GetActive().Count)));

            }


            return View(); // View'a döndürürken ilgili postu, yazarını(kullanıcıyı) döndürmemiz gerekecektir. Bu sebeple Tuple ya da ViewModel yapısını kullanabiliriz.


        }

        public IActionResult SearchResult(string q) // Kategorinin ID'si
        {
            // Kategori ID'ye göre aktif postları döndür.

            return View(postService.GetActive(x => x.Kullanici, x1=>x1.Comments).Where(x=>x.Title.Contains(q)|| x.PostDetail.Contains(q)).ToList());
        }

    }
}