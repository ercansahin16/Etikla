using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopNetCore.Data;
using ShopNetCore.Models;
using ShopNetCore.Service;
using System.Security.Claims;

namespace ShopNetCore.Controllers
{
  public class AdminController : Controller
  {
    UserService u = new();
    CategoryService cls = new();
    SupplierService s = new();
    StatusService st = new();
    ProductService p = new();
    ShopNetCoreContext context = new();

    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind("Email,Password,NameSurname")] User user)
    {

      if (ModelState.IsValid)
      {
        User? usr = await u.loginControl(user);
        if (usr != null)
        {
          var claims = new List<Claim>
          {
            new Claim(ClaimTypes.Role,"Admin")
          };
          var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
          var authProperties = new AuthenticationProperties();

          await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
            , new ClaimsPrincipal(claimsIdentity), authProperties
            );

          return RedirectToAction("Index", "Admin");
        }

      }
      else
      {
        ViewBag.error = "Login Bilgileri Yanlış";
      }

      return View();

    }
    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Login", "Admin");

    }
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
      return View();

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CategoryIndex()
    {
      List<Category> categories = await cls.GetCategoriesAsync();
      return View(categories);

    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult CategoryCreate()
    {
      CategoryFill();
      return View();

    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CategoryCreate(Category category)
    {
      bool answer = CategoryService.CategoryInsert(category);
      if (answer)
      {
        TempData["Message"] = "Kategori Eklendi";

      }
      else
      {
        TempData["Message"] = "HATA! Kategori Eklenemedi";
      }

      return RedirectToAction("CategoryCreate");
    }
    void CategoryFill() //parentID 0 olan kategorileri doldurur
    {
      List<Category> categories = cls.GetMainCategories();
      ViewData["categoryList"] = categories
        .Select(c =>
        new SelectListItem
        {
          Text = c.CategoryName,
          Value = c.CategoryID.ToString()
        });
    }

    async Task SuplierFill() //Markaları doldur
    {
      List<Supplier> suppliers = await s.GetSupplierAsync();
      ViewData["suppliersList"] = suppliers
        .Select(s =>
        new SelectListItem
        {
          Text = s.BrandName,
          Value = s.SupplierID.ToString()
        });
    }
    async Task StatusFill() //Durumları doldur
    {
      List<Status> statuses = await st.GetStatusAsync();
      ViewData["statusesList"] = statuses
        .Select(st =>
        new SelectListItem
        {
          Text = st.StatusName,
          Value = st.StatusID.ToString()
        });
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CategoryEdit(int? id)
    {
      CategoryFill();

      if (id == null || context.Categories == null)
      {

        return NotFound();
      }

      var category = await cls.GetCategoryDetailsAsync(id);
      return View(category);
    }
    [Authorize(Roles = "Admin")]

    [HttpPost]
    public IActionResult CategoryEdit(Category category)
    {
      bool answer = CategoryService.CategoryUpdate(category);
      if (answer)
      {
        TempData["Message"] = "Kategori Güncellendi";
        return RedirectToAction("CategoryIndex");
      }
      else
      {
        TempData["Message"] = "HATA! Kategori Güncellenemedi";
      }

      return RedirectToAction("CategoryEdit");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CategoryDetails(int id)
    {
      var category = await cls.GetCategoryDetailsAsync(id);
      ViewBag.Category = category?.CategoryName;
      return View(category);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> CategoryDelete(int? id)
    {
      if (id == null || context.Categories == null)
      {
        return NotFound();

      }

      var category = await context.
          Categories
          .FirstOrDefaultAsync(c => c.CategoryID == id);

      if (category == null)
      {
        return NotFound();
      }
      return View(category);


    }
    [HttpPost, ActionName("CategoryDelete")]
    [Authorize(Roles = "Admin")]
    public IActionResult CategoryDelete(int id)
    {

      bool answer = CategoryService.CategoryDelete(id);
      if (answer)
      {
        TempData["Message"] = "Kategori Silindi";
        return RedirectToAction("CategoryIndex");

      }
      else
      {
        TempData["Message"] = "Kategori Silinemedi";

      }
      return RedirectToAction("CategoryDelete");

    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SupplierIndex()
    {
      List<Supplier> suppliers = await s.GetSupplierAsync();
      return View(suppliers);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult SupplierCreate()
    {

      return View();

    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult SupplierCreate(Supplier supplier)
    {
      bool answer = SupplierService.SupplierInsert(supplier);
      if (answer)
      {
        TempData["Message"] = "Marka Eklendi";

      }
      else
      {
        TempData["Message"] = "HATA! Marka Eklenemedi";
      }

      return RedirectToAction("SupplierCreate");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SupplierEdit(int? id)
    {
      if (id == null || context.Suppliers == null)
      {

        return NotFound();
      }
      var supplier = await s.GetSupplierDetailsAsync(id);
      return View(supplier);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult SupplierEdit(Supplier supplier)
    {
      if (supplier.PhotoPath == null)
      {
        // Kullanıcı fotoğraf değiştirmez eski haliyle update yapar
        string? photoPath = context
            .Suppliers
            .FirstOrDefault(s => s.SupplierID == supplier.SupplierID)?.PhotoPath;


        supplier.PhotoPath = photoPath;

      }

      bool answer = SupplierService.SupplierUpdate(supplier);
      if (answer)
      {
        TempData["Message"] = supplier.BrandName?.ToUpper() + " Marka Güncellendi";
      }
      else
      {
        TempData["Message"] = "Marka Güncellenemedi";
      }
      return RedirectToAction("SupplierIndex");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SupplierDetails(int? id)
    {
      if (id == null)
        return NotFound();
      var supplier = await s.GetSupplierDetailsAsync(id);
      ViewBag.supplier = supplier?.BrandName;
      return View(supplier);
    }

    // SupplierDelete

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SupplierDelete(int? id)
    {
      if (id == null || context.Suppliers == null)
        return NotFound();

      var supplier = await context
          .Suppliers
          .FirstOrDefaultAsync(x => x.SupplierID == id);

      if (supplier == null)
        return NotFound();

      return View(supplier);

    }

    [HttpPost, ActionName("SupplierDelete")]
    [Authorize(Roles = "Admin")]
    public IActionResult SupplierDelete(int id)
    {
      bool answer = SupplierService.SupplierDeleteConfirmed(id);

      if (answer)
        TempData["Message"] = "Marka Silindi";
      else
        TempData["Message"] = "HATA! Marka Silinemedi";
      return RedirectToAction("SupplierIndex");
    }
    // Durum Formları => listeleme,ekleme,düzeltme,silme
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> StatusIndex()
    {
      List<Status> statuses = await st.GetStatusAsync();
      return View(statuses);
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult StatusCreate()
    {

      return View();

    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult StatusCreate(Status status)
    {
      bool answer = StatusService.StatusInsert(status);
      if (answer)
      {
        TempData["Message"] = "Durum Eklendi";

      }
      else
      {
        TempData["Message"] = "HATA! Durum Eklenemedi";
      }

      return RedirectToAction("StatusCreate");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> StatusEdit(int? id)
    {
      if (id == null || context.Statuses == null)
      {
        return NotFound();
      }
      var status = await st.GetStatusDetailsAsync(id);
      return View(status);

    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult StatusEdit(Status status)
    {
      if (status == null)
        return NotFound();



      bool answer = StatusService.StatusUpdate(status);
      if (answer)
        TempData["Message"] = "Durum Güncellemesi Yapıldı";
      else
        TempData["Message"] = "HATA! Durum Güncellemesi Yapılamadı";
      return RedirectToAction("StatusIndex");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> StatusDelete(int? id)
    {
      if (id == null || context.Statuses == null)
        return NotFound();

      var status = await context.Statuses.FirstOrDefaultAsync(st => st.StatusID == id);
      if (status == null)
        return NotFound();
      return View(status);

    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult StatusDelete(int id)
    {
      bool answer = StatusService.StatusDeleteConfirmed(id);
      if (answer)
        TempData["Message"] = "Durum Silindi";
      else
        TempData["Message"] = "HATA! Durum Silinemedi";
      return RedirectToAction("StatusIndex");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> StatusDetails(int? id)
    {
      if (id == null)
        return NotFound();

      var status = await st.GetStatusDetailsAsync(id);
      return View(status);
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProductIndex()
    {
      List<Product> products = await p.GetProductsAsync();
      return View(products);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ProductCreate()
    {
      CategoryFill();
      await StatusFill();
      await SuplierFill();
      return View();

    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult ProductCreate(Product product)
    {
      if (ModelState.IsValid)
      {
        bool answer = ProductService.ProductInsert(product);

        if (answer)
          TempData["Message"] = "Ürün Eklendi";
        else
          TempData["Message"] = "Hata Ürün Eklenemedi";
      }
      else
        TempData["Message"] = "Zorunlu Alanları Doldurunuz.";



      return RedirectToAction("ProductCreate"); //["HTTPGET"]
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ProductEdit(int? id)
    {
      CategoryFill();
      await SuplierFill();
      await StatusFill();

      if (id == null || context.Products == null)
        return NotFound();
      var products = await p.GetProductsDetailsAsync(id);
      return View(products);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> ProductEdit(Product product)
    {
      Product? existProduct = await p.GetProductsDetailsAsync(product.ProductID);
      if (product.PhotoPath == null)
      {
        //fotoğraf değişmez ise eski haliyle kaydetsin
        //string? photoPath = context
        //   .Products
        //   .FirstOrDefault(x => x
        //   .ProductID == product.ProductID)?.PhotoPath;


        product.PhotoPath = existProduct?.PhotoPath;

      }

      // bana gelmeyen değerler: highlighted,topseller,adddate

      product.HighLighted = existProduct!.HighLighted;
      product.TopSeller = existProduct.TopSeller;
      product.AddDate = existProduct.AddDate;

      bool answer = ProductService.ProductUpdate(product);

      if (answer)
      {
        TempData["Message"] = "Ürün Güncellendi";
        return RedirectToAction("ProductIndex");
      }
      else
        TempData["Message"] = "HATA! Ürün Güncellenemedi";


      return RedirectToAction("ProductEdit");
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ProductDelete(int? id)
    {
      if (id == null || context.Products == null)
        return NotFound();

      var product = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
      if (product == null)
        return NotFound();
      return View(product);

    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult ProductDelete(int id)
    {
      bool answer = ProductService.ProductDeleteConfirmed(id);
      if (answer)
        TempData["Message"] = "Ürün Silindi";
      else
        TempData["Message"] = "HATA! Ürün Silinemedi";
      return RedirectToAction("ProductDelete");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProductDetails(int? id)
    {
      var product = await p.GetProductsDetailsAsync(id);
      return View(product);
    }










  }
}
