using BlogProject.Core.Entity.Enum;
using BlogProject.Core.Service;
using BlogProject.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_BlogProject.Areas.Administrator.Controllers
{
    [Area("Administrator"), Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly ICoreService<User> userService;

        public UserController(ICoreService<User> _userService)
        {
            userService = _userService;
        }
        public IActionResult Index()
        {
            return View(userService.GetAll());
        }
        public IActionResult Activate(Guid id) // Gelen ID'ye göre ilgili nesneyi aktifleştirecek.
        {
            // View göstermeyecek direkt Index'e yönlendirebiliriz.
            userService.Activate(id);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id) // Gelen ID'ye göre ilgili nesneyi silecek.
        {
            // View göstermeyecek direkt Index'e yönlendirebiliriz.
            userService.Remove(userService.GetById(id));
            return RedirectToAction("Index");
        }
        public IActionResult SetAdmin(Guid id) // Gelen ID'ye göre ilgili nesneyi aktifleştirecek.
        {
            // View göstermeyecek direkt Index'e yönlendirebiliriz.
            User adminUser = userService.GetById(id);
            adminUser.UserRole = UserRole.Admin;
            userService.Update(adminUser);
            return RedirectToAction("Index");
        }
    }
}
