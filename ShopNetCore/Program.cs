using Microsoft.AspNetCore.Authentication.Cookies;
using ShopNetCore.Hubs;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
{
  opt.Cookie.Name = "ShopNetCore.Auth";
  opt.LoginPath = "/Admin/Login";

  opt.LogoutPath = "/Admin/Login";

  opt.AccessDeniedPath = "/Admin/Login";
});

// süre olarak 1 dakika belirledik
builder.Services.AddSession(option =>
{
  option.IdleTimeout = TimeSpan.FromMinutes(10);
});
// Alert türkçe karakter sorununu çözmek için eklendi
builder.Services.AddWebEncoders(o =>
{
  o.TextEncoderSettings = new System.Text.Encodings.Web.TextEncoderSettings(UnicodeRanges.All);
});

// layoutda session login görünümü için
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession(); // ekledim

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<AdminHub>("/adminHub");

app.Run();
