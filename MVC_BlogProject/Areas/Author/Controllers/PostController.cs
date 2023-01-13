using BlogProject.Core.Entity.Enum;
using BlogProject.Core.Service;
using BlogProject.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_BlogProject.Areas.Author.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MVC_BlogProject.Areas.Author.Controllers
{
    [Area("Author"), Authorize(Roles = "Yazar")]

    public class PostController : Controller
    {
        private readonly ICoreService<Post> postService;
        private readonly ICoreService<Category> categoryService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment environment;

        public PostController(ICoreService<Post> postService, ICoreService<Category> categoryService, IHostingEnvironment environment)
        {
            this.postService = postService;
            this.categoryService = categoryService;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            return View(postService.GetDefault(x => x.UserID == Guid.Parse(HttpContext.User.FindFirst("ID").Value.ToString())));
        }


        [HttpGet] // Create sayfasını gösterecek.
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(categoryService.GetActive(), "ID", "CategoryName");
            return View();
        }


        [HttpPost] // Create sayfasından gelen veriyi DB'ye ekleyecek.
        public IActionResult Create(Post post, List<IFormFile> files)
        {
            ViewBag.Categories = new SelectList(categoryService.GetActive(), "ID", "CategoryName");

            post.UserID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value);

            // Alınan görselleri, görsel yüklemek için oluşturduğumuz metoda göndereceğiz.
            bool imageResult;
            string imagePath = Upload.ImageUpload(files, environment, out imageResult);

            if (imageResult)
            {
                post.ImagePath = imagePath; // Eğer imageResult true ise ilgili property'e resmin yolunu ekle.
            }
            else
            {
                ViewBag.MessageError = $"Görsel yükleme sırasında hata oluştu!";
                return View();
            }

            post.Status = Status.None;

            if (ModelState.IsValid)
            {
                bool result = postService.Add(post);
                if (result)
                {
                    TempData["MessageSuccess"] = $"Kayıt işlemi başarılı.";
                    return RedirectToAction("Index"); // Ekleme işlemi başarılı ise Index'e döndürebiliriz.

                }
                else
                {
                    TempData["MessageError"] = $"Kayıt işlemi sırasında bir hata meydana geldi. Lütfen tüm alanları kontrol edin.";
                }
            }
            else
            {
                TempData["MessageError"] = $"Kayıt işlemi sırasında bir hata meydana geldi. Lütfen tüm alanları kontrol edin.";
            }
            return View(post); // Ekleme işlemi sırasında kullanılan category bilgileriyle View'a döndürmesi sağlanabilir.
        }




        [HttpGet] // İlgili nesne ile update sayfasını gösterecek.
        public IActionResult Update(Guid id)
        {
            ViewBag.Categories = new SelectList(categoryService.GetActive(), "ID", "CategoryName");

            return View(postService.GetById(id));
        }


        [HttpPost] // Update sayfasından gelen veriyi DB'de güncelleyecek.
        public IActionResult Update(Post post, List<IFormFile> files)
        {
            ViewBag.Categories = new SelectList(categoryService.GetActive(), "ID", "CategoryName");

            // Alınan görselleri, görsel yüklemek için oluşturduğumuz metoda göndereceğiz.
            bool imageResult;
            string imagePath = Upload.ImageUpload(files, environment, out imageResult);

            if (imageResult)
            {
                post.ImagePath = imagePath; // Eğer imageResult true ise ilgili property'e resmin yolunu ekle.
            }
            else
            {
                ViewBag.MessageError = $"Görsel yükleme sırasında hata oluştu!";
                return View();
            }


            if (ModelState.IsValid)
            {
                Post updatedPost = postService.GetById(post.ID);
                updatedPost.Title = post.Title;
                updatedPost.PostDetail = post.PostDetail;
                updatedPost.ImagePath = post.ImagePath;
                updatedPost.Tags = post.Tags;
                updatedPost.CategoryID = post.CategoryID;

                updatedPost.Status = Status.Updated;

                bool result = postService.Update(updatedPost);
                if (result)
                {
                    postService.Save();
                    return RedirectToAction("Index"); // Ekleme işlemi başarılı ise Index'e döndürebiliriz.

                }
                else
                {
                    TempData["MessageError"] = $"Kayıt işlemi sırasında bir hata meydana geldi. Lütfen tüm alanları kontrol edin.";
                }
            }
            else
            {
                TempData["MessageError"] = $"Kayıt işlemi sırasında bir hata meydana geldi. Lütfen tüm alanları kontrol edin.";
            }
            return View(post); // Ekleme işlemi sırasında kullanılan category bilgileriyle View'a döndürmesi sağlanabilir.
        }
    }
}