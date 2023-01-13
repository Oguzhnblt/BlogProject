using BlogProject.Core.Entity.Enum;
using BlogProject.Core.Service;
using BlogProject.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_BlogProject.Areas.Administrator.Models.ViewModels;

namespace MVC_BlogProject.Areas.Administrator.Controllers
{
    [Area("Administrator"), Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
        private readonly ICoreService<Comment> commentService;
        private readonly ICoreService<Post> postService;
        private readonly ICoreService<User> userService;

        public CommentController(ICoreService<Comment> _commentService, ICoreService<Post> _postService, ICoreService<User> _userService)
        {
            commentService = _commentService;
            postService = _postService;
            userService = _userService;
        }
        public IActionResult Index()
        {
            return View(commentService.GetAll(x => x.User, a => a.Post).ToList());

        }
        public IActionResult CommentDetail(Guid id)
        {
            CommentDetailVM vm = new CommentDetailVM();
            vm.Comment = commentService.GetById(id);
            vm.User = userService.GetById(commentService.GetById(id).UserID);
            vm.Post = postService.GetById(commentService.GetById(id).PostID);

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(Guid userID, Guid postID, string commentMessage)
        {
            Comment addedComment = new Comment();
            addedComment.UserID = userID;
            addedComment.PostID = postID;
            addedComment.CommentText = commentMessage;

            addedComment.Status = Status.None;

            if (ModelState.IsValid)
            {
                bool result = commentService.Add(addedComment);
                if (result)
                {
                    TempData["MessageSuccess"] = $"Yorumunuz onaydan sonra yayınlanacaktır.";

                    return RedirectToAction("Post", "Home", new { area = "", id = postID }); // Ekleme işlemi başarılı ise Index'e döndürebiliriz.

                }
                else
                {
                    TempData["MessageError"] = $"Yorumunuz onaydan.";
                }
            }
            else
            {
                TempData["MessageError"] = $"Kayıt işlemi sırasında bir hata meydana geldi. Lütfen tüm alanları kontrol edin.";
            }
            return RedirectToAction("Post", "Home", new { area = "", id = postID }); // Ekleme işlemi sırasında kullanılan Comment bilgileriyle View'a 



        }

        public IActionResult Activate(Guid id) // Gelen ID'ye göre ilgili nesneyi aktifleştirecek.
        {
            // View göstermeyecek direkt Index'e yönlendirebiliriz.
            commentService.Activate(id);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id) // Gelen ID'ye göre ilgili nesneyi silecek.
        {
            // View göstermeyecek direkt Index'e yönlendirebiliriz.
            commentService.Remove(commentService.GetById(id));
            return RedirectToAction("Index");
        }
    }
}
