using BlogProject.Core.Entity.Enum;
using BlogProject.Core.Service;
using BlogProject.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_BlogProject.Areas.Administrator.Controllers
{
	[Area("Administrator"), Authorize(Roles ="Admin")]

	public class CategoryController : Controller
	{
		private readonly ICoreService<Category> _categoryService;
		public CategoryController(ICoreService<Category> categoryService)
		{
			_categoryService = categoryService;
		}
		public IActionResult Index()
		{
			return View(_categoryService.GetAll());
		}


		[HttpGet] // Create sayfasını gösterecek.
		public IActionResult Create()
		{
			return View();
		}


		[HttpPost] // Create sayfasından gelen veriyi DB'ye ekleyecek.
		public IActionResult Create(Category category)
		{
			category.Status = Status.None;

			if (ModelState.IsValid)
			{
				bool result = _categoryService.Add(category);
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
			return View(category); // Ekleme işlemi sırasında kullanılan category bilgileriyle View'a döndürmesi sağlanabilir.
		}




		[HttpGet] // İlgili nesne ile update sayfasını gösterecek.
		public IActionResult Update(Guid id)
		{
            return View(_categoryService.GetById(id));
        }


        [HttpPost] // Update sayfasından gelen veriyi DB'de güncelleyecek.
		public IActionResult Update(Category category)
		{
			Category updatedCategory = _categoryService.GetById(category.ID);
			updatedCategory.Description = category.Description;
			updatedCategory.CategoryName = category.CategoryName;
            updatedCategory.Status = Status.Updated;

            if (ModelState.IsValid)
            {
                bool result = _categoryService.Update(updatedCategory);
                if (result)
                {
					_categoryService.Save();
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
            return View(category); // Ekleme işlemi sırasında kullanılan category bilgileriyle View'a döndürmesi sağlanabilir.
        }


		public IActionResult Activate(Guid id) // Gelen ID'ye göre ilgili nesneyi aktifleştirecek.
		{
			// View göstermeyecek direkt Index'e yönlendirebiliriz.
			_categoryService.Activate(id);
			return RedirectToAction("Index");
		}
		public IActionResult Delete(Guid id) // Gelen ID'ye göre ilgili nesneyi silecek.
		{
			// View göstermeyecek direkt Index'e yönlendirebiliriz.
			_categoryService.Remove(_categoryService.GetById(id));
			return RedirectToAction("Index");
		}
	}
}
