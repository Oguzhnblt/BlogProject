using BlogProject.Core.Entity.Enum;
using BlogProject.Core.Service;
using BlogProject.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using MVC_BlogProject.Models.ViewModels;

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

            Post readPost = postService.GetById(id);
            readPost.ViewCount++;
            postService.Update(readPost);

            PostDetailVM vm = new PostDetailVM();
            vm.Post = readPost;
            vm.Category = categoryService.GetById(readPost.CategoryID);
            vm.User = userService.GetById(readPost.UserID);
            vm.Comments = commentService.GetDefault(x => x.Status == Status.Active && x.PostID == readPost.ID);
            vm.Categories = categoryService.GetActive();



            // Random RelatedPost döndürmek için

            Random r = new Random(); // Random nesnesi oluşturduk.

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

            return View(postService.GetActive(x => x.User, x1 => x1.Comments).Where(x => x.Title.Contains(q) || x.PostDetail.Contains(q)).ToList());
        }

    }
}