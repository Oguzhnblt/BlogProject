using IHostEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace MVC_BlogProject.Areas.Administrator.Models
{
    public class Upload
    {
        public static string ImageUpload(List<IFormFile> files, IHostEnvironment _env, out bool result)
        {
            // Görsel yükleme işlemlerini gerçekleştireceğiz. Geriye görsel yolunu veya hata mesajını döndüreceğiz. Ayrıca, dönen string'in bilgisi mi yoksa hata mesajı mı olduğunu anlamak için dışarıya result değeri fırlatacağız.

            result = false;
            var uploads = Path.Combine(_env.WebRootPath, "Uploads");

            foreach (var file in files)
            {
                if (file.ContentType.Contains("image")) // Dosyanın İmage tipinde olması lazım
                {
                    if (file.Length <= 2097152) // Dosya boyutu 2 MB ve daha küçükse
                    {
                        string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{file.ContentType.Split("/")[1]}";
                        var filePath = Path.Combine(uploads, uniqueName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            result = true;
                            string newFilePath =  filePath.Substring(filePath.IndexOf("Uploads\\"));
                            return newFilePath.Replace("\\", "/");
                        }
                    }
                    else
                    {
                        return $"2 MB'den büyük boyutta görsel yükleyemezsiniz.";
                    }
                }
                else
                {
                    return $"Lütfen sadece görsel yükleyiniz.";
                }
            }
            return "Dosya bulunamadı. Lütfen en az bir tane dosya seçiniz.";
        }
    }
}
