using BlogProject.Core.Entity.Enum;
using BlogProject.Core.Service;
using BlogProject.Entities.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVC_BlogProject.Controllers
{

    public class AccountController : Controller
    {
        private readonly ICoreService<User> userService;

        public AccountController(ICoreService<User> _userService)
        {
            userService = _userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            // Kullanıcının DB'de olup olmadığını kontrol ediyoruz.

            if (userService.Any(x => x.EmailAddress == user.EmailAddress && x.Password == user.Password && x.Status == Status.Active))
            {
                // Eğer kullanıcı DB'de var ise kullanıcıyı yakalıyoruz. 

                User loggedUser = userService.GetByDefault(x => x.EmailAddress == user.EmailAddress && x.Password == user.Password);

                // Kullanıcımızın saklayacağımız bilgilerini Claim'ler olarak tutabiliriz.

                var claims = new List<Claim>()
                {
                    new Claim("ID",loggedUser.ID.ToString()),
                    new Claim(ClaimTypes.Name,loggedUser.FirstName),
                    new Claim(ClaimTypes.Surname,loggedUser.LastName),
                    new Claim(ClaimTypes.Email,loggedUser.EmailAddress),
                    new Claim("ImageURL",loggedUser.ImageURL),
                    new Claim(ClaimTypes.Role,loggedUser.UserRole.ToString())

                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal); // Kullanıcı girişi yaptırılır.

                // Yönetici Home/Index sayfasına yönlendirilir.

                if (loggedUser.UserRole == UserRole.Yazar)
                {
                    return RedirectToAction("Index", "Home", new { area = "Author" });
                }
                else if (loggedUser.UserRole == UserRole.Admin)
                {
                    return RedirectToAction("Index", "Home", new { area = "Administrator" });
                }

            }

            // Eğer kullanıcı, bilgileri ile giriş yapamazsa form'a geri dönmesi için
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" }); // Çıkış yapıldıktan sonra blog anasayfasına yönlendirmek için
        }




        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(User user)
        {

            user.Title = "Yazar";
            user.Status = Status.Active;
            user.ImageURL = "Uploads/blog-user.jpg";
            
            if (ModelState.IsValid)
            {
                bool result = userService.Add(user);
                if (result)
                {
                    TempData["MessageSuccess"] = $"Kayıt işlemi başarılı.";
                    return RedirectToAction("Index", "Home", new { area = "" }); // Ekleme işlemi başarılı ise Index'e döndürebiliriz.

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
            return View(user); // Ekleme işlemi sırasında kullanılan bilgileriyle View'a döndürmesi sağlanabilir.
        }
    }
}
