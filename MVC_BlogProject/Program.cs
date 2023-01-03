using BlogProject.Core.Service;
using BlogProject.Entities.Context;
using BlogProject.Service.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


// .NET Core MVC'de tamamen Dependency Injection yap�s�yla �al���yoruz. ICoreService interface'inin BaseService ile olan gev�ek ba��ml�l���n� tan�ml�yoruz. Nerede ICoreService �a��r�l�rsa, onun yerine BaseService g�nderilecektir.

builder.Services.AddScoped(typeof(ICoreService<>), typeof(BaseService<>));


builder.Services.AddDbContext<BlogProjectContext>(options => options.UseSqlServer("Server=OGUZ; Database=OnionBlogProject; uid=sa; pwd=123;"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
