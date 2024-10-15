using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ShopNetCore.Data;
using ShopNetCore.Hubs;
using ShopNetCore.MainPModel;
using ShopNetCore.Models;
using ShopNetCore.Service;
using ShopNetCore.ViewModels;
using System.Collections.Specialized;
using System.Text;

namespace ShopNetCore.Controllers
{
  public class HomeController : Controller
  {
    /*
       1-** Slider
       2- **Özel Ürün
       3- **Yıldızlı
       4- **Fırsat
       5- Dikkat Çeken
       6-*** Günün Ürünü

       addDate** = Yeni Ürünler
       ** discount= İndirimli
       **highlighted = öne çıkanlar
       **topseller=en çok satanlar
     */

    ShopNetCoreContext context = new();
    MainPageModel mpm = new();
    ProductService cp = new();
    OrderService order = new();
    SupplierService suplier = new();
    CategoryService category = new();
    IHubContext<AdminHub> hubContext;

    public HomeController(IHubContext<AdminHub> hubContext)
    {
      this.hubContext = hubContext;
    }

    public async Task<IActionResult> Index()
    {
      await hubContext.Clients.All.SendAsync("KullaniciHareketiniYakala", "Ziyaretçi", "Siteye giriş yaptı");

      mpm.SliderProducts = cp.GetProducts("SliderProducts", "index");             //slider ürün
      mpm.NewProducts = cp.GetProducts("NewProducts", "index");                  //Yeni ürün
      mpm.ProductOfDay = cp.GetProductOfDay();                                   //Günün Ürünü
      mpm.SpecialProducts = cp.GetProducts("SpecialProducts", "index");          //Özel ürün
      mpm.StarProducts = cp.GetProducts("StarProducts", "index");                 // Yıldızlı ürün
      mpm.FeaturedProducts = cp.GetProducts("FeaturedProducts", "index");         //Fırsat ürün
      mpm.DiscountedProducts = cp.GetProducts("DiscountedProducts", "index");    // İndirimli
      mpm.HighlightedProducts = cp.GetProducts("HighlightedProducts", "index");  // Öne Çıkan
      mpm.TopSellerProducts = cp.GetProducts("TopSellerProducts", "index");      // Çok Satan
      mpm.NotableProducts = cp.GetProducts("NotableProducts", "index");          // Dikkat Çeken
      return View(mpm);
    }
    public IActionResult NewProducts()
    {
      mpm.NewProducts = cp.GetProducts("NewProducts", "topmenu");         //Alt sayfa ,menüden yeni ürünlere tıklayınca

      return View(mpm);
    }
    public PartialViewResult _PartialNewProducts(string nextpagenumber)
    {
      //nextpagenumber * 4 = kaçıncı üründen başlayacak Skip
      int pagenumber = Convert.ToInt32(nextpagenumber);
      mpm.NewProducts = cp.GetProducts("NewProducts", "topmenuajax", pagenumber); //Alt sayfa , daha fazla butonu tıklayınca

      return PartialView(mpm);
    }
    public IActionResult SpecialProducts()
    {
      mpm.SpecialProducts = cp.GetProducts("SpecialProducts", "topmenu");         //Alt sayfa ,menüden özelürünlere tıklayınca

      return View(mpm);
    }

    public PartialViewResult _PartialSpecialNewProducts(string nextpagenumber)
    {
      //nextpagenumber * 4 = kaçıncı üründen başlayacak Skip
      int pagenumber = Convert.ToInt32(nextpagenumber);
      mpm.SpecialProducts = cp.GetProducts("SpecialProducts", "topmenuajax", pagenumber); //Alt sayfa , daha fazla butonu tıklayınca

      return PartialView(mpm);
    }

    public IActionResult DiscountedProducts()
    {
      mpm.DiscountedProducts = cp.GetProducts("DiscountedProducts", "topmenu");         //Alt sayfa ,menüden özelürünlere tıklayınca

      return View(mpm);
    }

    public PartialViewResult _PartialDiscountedProducts(string nextpagenumber)
    {
      //nextpagenumber * 4 = kaçıncı üründen başlayacak Skip
      int pagenumber = Convert.ToInt32(nextpagenumber);
      mpm.DiscountedProducts = cp.GetProducts("DiscountedProducts", "topmenuajax", pagenumber); //Alt sayfa , scroll aşağı inince

      return PartialView(mpm);
    }
    public IActionResult HighlightedProducts()
    {
      mpm.HighlightedProducts = cp.GetProducts("HighlightedProducts", "topmenu");         //Alt sayfa ,menüden özel ürünlere tıklayınca

      return View(mpm);
    }
    public PartialViewResult _PartialHighlightedProducts(string nextpagenumber)
    {
      //nextpagenumber * 4 = kaçıncı üründen başlayacak Skip
      int pagenumber = Convert.ToInt32(nextpagenumber);
      mpm.HighlightedProducts = cp.GetProducts("HighlightedProducts", "topmenuajax", pagenumber); //Alt sayfa , scroll aşağı inince

      return PartialView(mpm);
    }
    public IActionResult TopSellerProducts(int page = 1, int pageSize = 16)
    {
      PagedList<Product> products = new PagedList<Product>(

        context.Products.Where(x => x.Active == true).OrderByDescending(x => x.TopSeller),
        page, pageSize

        );

      return View(products);
    }

    public IActionResult CategoryPage(int id)
    {
      //mpm.ProductsByCategory = cp.GetProductsByCategory(id);
      List<Product> products = cp.GetProductsByCategory(id);


      var category = context.Categories.FirstOrDefault(x => x.CategoryID == id);
      var CountID = Enumerable.Range(1, 500);
      if (CountID.Contains(id))
      {
        ViewData["TitlePage"] = category?.CategoryName;
      }
      return View(products);


    }

    public IActionResult SupplierPage(int id)
    {
      List<Product> products = cp.GetProductsBySupplier(id);
      var supplier = context.Suppliers.FirstOrDefault(x => x.SupplierID == id);
      var CountID = Enumerable.Range(1, 500);
      if (CountID.Contains(id))
      {
        ViewData["TitlePage"] = supplier?.BrandName;
      }
      return View(products);
    }

    // Projenin herhangi bir sayfasında sepete ekle butonu tıklanınca çalışacak
    public IActionResult CartProcess(int id)
    {
      // highlighted kolonunu 1 arttır 
      cp.HighlightedPlus(id);

      order.ProductID = id;
      order.Quantity = 1;

      var cookieOptions = new CookieOptions(); // nesne oluşturuk, instance aldık

      // çerez politikası
      var cookie = Request.Cookies["SPT"]; //tarayıcıda SPT isminde cookie var mı diye bakıyoruz
      if (cookie == null)
      {
        //kullanıcı sisteme ilk defa ürün ekleyecek
        cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTime.Now.AddDays(1); // 1 günlük çerez tanımladık (geriye dönük için - li değer)
        cookieOptions.Path = "/";
        order.Sepet = "";
        order.AddToCart(id.ToString()); //Sepete ekle metodu

        Response.Cookies.Append("SPT", order.Sepet, cookieOptions);//tanımladığımız çerezi tarayıcıya gönderdik
        HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
        TempData["Message"] = "Ürün Sepetinize Eklendi";
      }
      else
      {
        order.Sepet = cookie; //kullanıcı daha önceden sepetine ürün eklemiş tarayıcıdaki sepetim içeriğini property'ye gönderiyoruz

        // aynı ürün daha önceden sepete eklenmiş mi
        if (order.AddToCart(id.ToString()) == false)
        {
          Response.Cookies.Append("SPT", order.Sepet, cookieOptions); //eklenmemiş
          cookieOptions.Expires = DateTime.Now.AddDays(1);
          // HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
          TempData["Message"] = "Ürün Sepetinize Eklendi";
        }
        else
        {
          // HttpContext.Session.SetString("Message", "Bu ürün Sepetinizde Zaten Var");
          TempData["Message"] = "Bu ürün Sepetinizde Zaten Var";

        }
      }
      string url = Request.Headers["Referer"].ToString(); // ürünü sepete ekledikten sonra hangi sayfadaysa oraya dönsün
      return Redirect(url);
    }
    public async Task<IActionResult> Details(int id)
    {
      //efcore
      //mpm.ProductDetails = context.Products.FirstOrDefault(p => p.ProductID == id);

      //select * from Products where ProductID = id --> ado.net,dapper

      //LİNQ - id'si 4 olan ürünün bütün kolon (sütun) bilgilerini getir
      mpm.ProductDetails = (from p
                         in context.Products
                            where p.ProductID == id
                            select p)
                         .FirstOrDefault();
      //LINQ
      mpm.CategoryName = (from p in context.Products
                          join c in context.Categories
                          on p.CategoryID equals c.CategoryID
                          where p.ProductID == id
                          select c.CategoryName).FirstOrDefault();
      //LINQ
      mpm.BrandName = (from p in context.Products
                       join s in context.Suppliers
                       on p.SupplierID equals s.SupplierID
                       where p.ProductID == id
                       select s.BrandName).FirstOrDefault();

      //select * from Products where Related = x and ProductID != id
      mpm.RelatedProducts = context
            .Products
            .Where(p =>
              p.Related == mpm.ProductDetails!.Related &&
              p.ProductID != id)
            .ToList();
      mpm.Comments = context.Comments.Where(c => c.ProductID == id)
        .OrderByDescending(c => c.AddDate).ToList();

      ViewBag.Categories = await category.GetCategoriesAsync();
      ViewBag.Supliers = await suplier.GetSuppliersAsync();
      //highlighted kolonunu 1 arttır
      cp.HighlightedPlus(id);

      return View(mpm);
    }

    //Projenin sağ üst köşesinden sepet sayfama git tıklanınca
    // sepetten ürün silerken sil butonuna tıklayınca
    public IActionResult Cart()
    {
      if (HttpContext.Request.Query["scid"].ToString() != "")
      {
        // sayfada ürün silerken sil butonuna tıklanınca scid göndereceğiz
        // sepetim cookie (çerez)'sinden ürün silerek cart.cshtml'e gidecek
        int scid = Convert.ToInt32(HttpContext.Request.Query["scid"].ToString());
        order.Sepet = Request.Cookies["SPT"]; //tarayıcıdan aldık property'e koyduk
        order.DeleteFromMyCart(scid.ToString());

        var cookieOptions = new CookieOptions();
        cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTime.Now.AddDays(1); // 1 günlük çerez tanımladık (geriye dönük için - li değer)
        Response.Cookies.Append("SPT", order.Sepet!, cookieOptions);//tanımladığımız çerezi tarayıcıya gönderdik
        TempData["Message"] = "Ürün Sepetinizden Kaldırıldı";

        //cart.cshtml ürünleri foreach ile ürünleri dönüp gösterecek
        List<OrderService> listBasket = order.GetMyCart();
        ViewBag.basket = listBasket;
        ViewBag.basket_table_detail = listBasket;
        if (listBasket.Count == 0)
        {
          ViewBag.bossepet = "BOŞ";
        }

      }
      else
      {
        // sepetim cookie (çerez)'sinden hibir şey değiştirmeden cart.cshtml'e gidecek
        // Projenin sağ üst köşesinden sepet sayfama git tıklanınca 
        var cookie = Request.Cookies["SPT"];
        List<OrderService> basket;
        var cookieOptions = new CookieOptions();
        if (cookie == null)
        {

          order.Sepet = "";
          basket = order.GetMyCart();
          ViewBag.basket = basket;
          ViewBag.basket_table_detail = basket;
          if (basket.Count == 0)
          {
            ViewBag.bossepet = "BOŞ";
          }
        }
        else
        {
          order.Sepet = Request.Cookies["SPT"];
          basket = order.GetMyCart();
          ViewBag.basket = basket;
          ViewBag.basket_table_detail = basket;
          if (basket.Count == 0)
          {
            ViewBag.bossepet = "BOŞ";
          }

        }

      }
      return View();
    }
    [HttpGet]
    public IActionResult Order()
    {
      //kullanıcı login girişi yapmış mı
      if (HttpContext.Session.GetString("Email") != null)
      {
        // bu kullanıcı login girişi yapmış ve benden Email isminde bir oturum almış
        User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email"));
        return View(user);
      }
      else
      {
        return RedirectToAction("Login");
      }


    }
    [HttpPost]
    public IActionResult Order(Order order, IFormCollection frm)
    {
      if (Request.Form["txt_tckimlikno"] != "")
      {
        string? tckimlikno = Request.Form["txt_tckimlikno"];
      }
      if (frm["txt_vergino"] != "")
      {
        string? vergino = Request.Form["txt_vergino"];
      }

      string? kredikartno = Request.Form["kredikartno"];
      string? kredikartay = Request.Form["kredikartay"];
      string? kredikartyıl = Request.Form["kredikartyıl"];
      string? kredikartcvv = Request.Form["kredikartcvv"];

      // payu paytr paratika iyzico
      // gizlilik sözleşmesi, iade koşulları, telefon numarası

      NameValueCollection data = new NameValueCollection();
      string payu_url = "https://www.eticaret.com/backref";
      data.Add("BACK_REF", payu_url);
      data.Add("CC_CVV", kredikartcvv);
      data.Add("CC_NUMBER", kredikartno);
      data.Add("CC_MONTH", kredikartay);
      data.Add("CC_YEAR", kredikartyıl);

      var deger = "";
      foreach (var item in data)
      {
        var value = item as string;
        var byteCount = Encoding.UTF8.GetByteCount(data.Get(value)!);
        deger += byteCount + data.Get(value);
      }
      var signatureKey = "size verilen SECRET_KEY buraya yazılacak";
      var hash = HashWithSignature(deger, signatureKey);
      data.Add("ORDER_HASH", hash);
      var x = POSTFormPayu("https://secure.payu.com.tr/order/....", data);

      if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLL</RETURN_CODE>"))
      {
        // sanal kart ile alışverişini yaptı ve ok döndü
      }
      else
      {
        //gerçek kart
        if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>AUTHORİZED</RETURN_CODE>"))
        {
          // gerçek kart ile alışveriş yaptı ok döndü
        }
      }
      return RedirectToAction("backref");
    }
    public string POSTFormPayu(string url, NameValueCollection data)
    {
      return "";
    }
    public string HashWithSignature(string deger, string signatureKey)
    {
      return "";
    }
    public IActionResult backref()
    {
      OrderConfirm();
      return RedirectToAction("Confirm");
    }
    public static string OrderGroupGUID = ""; //20240827215030
    public static string cevap = ""; //netgsm'den dönen cevap için değişken
    public IActionResult OrderConfirm()
    {
      //cookie sepetindeki siparişi Orders tablosuna işleyeceğiz, sepeti sileceğiz

      var cookieOptions = new CookieOptions();

      var cookie = Request.Cookies["SPT"]; //tarayıcıda sepetim isiminde cookie(çerez) var mı diye  bakıyoruz

      if (cookie != null)
      {
        order.Sepet = cookie;
        OrderGroupGUID = order.WriteFromCookieToTable(HttpContext.Session.GetString("Email")!.ToString());
        cookieOptions.Expires = DateTime.Now.AddDays(1); //1 GÜNLÜK ÇEREZ DEĞERİ TANIMLADIK

        Response.Cookies.Delete("SPT");
        cevap = UserService.Send_Sms(OrderGroupGUID);
        UserService.Send_Email(OrderGroupGUID);

      }
      return RedirectToAction("Confirm");
    }
    public IActionResult Register()
    {
      return View();
    }
    [HttpPost]
    public IActionResult Register(User user)
    {
      string answer = UserService.AddUser(user);
      if (answer == "Başarılı")
      {
        TempData["Message"] = "Bilgileriniz başarıyla kaydedildi";
        return RedirectToAction("Login");
      }
      else if (answer == "Email zaten var")
      {
        TempData["Message"] = "Email daha önceden kayıtlı.Tekrar deneyin";
        return View();
      }
      else
      {
        TempData["Message"] = "HATA!";
      }
      return View();
    }
    public IActionResult Login()
    {
      string url = Request.Headers["Referer"].ToString();
      HttpContext.Session.SetString("url", url);
      return View();
    }
    [HttpPost]
    public IActionResult Login(User user)
    {
      string answer = UserService.UserControl(user);
      if (answer == "yanlış")
      {
        // email şifre kayıtlı değil
        TempData["Message"] = "HATA.. Email veya Şifreniz yanlış. Tekrar Deneyiniz";
        return View();
      }
      else if (answer == "admin")
      {
        HttpContext.Session.SetString("Admin", answer);
        return RedirectToAction("Login", "Admin");
      }
      else if (answer == "HATA")
      {
        TempData["Message"] = "BİR HATA OLUŞTU TEKRAR DENEYİN";
        return View();
      }
      else
      {
        HttpContext.Session.SetString("Email", answer);
        if (HttpContext.Session.GetString("url") != null)
        {
          string? url = HttpContext.Session.GetString("url");
          HttpContext.Session.Remove(url!);
          return Redirect(url!);
        }
        return RedirectToAction("Index", "Home");
      }
    }
    public IActionResult Logout()
    {
      HttpContext.Session.Remove("Email");
      HttpContext.Session.Remove("Admin");
      return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult AddToCart(int id)
    {
      string message = "";
      // highlighted kolonunu 1 arttır 
      cp.HighlightedPlus(id);

      order.ProductID = id;
      order.Quantity = 1;

      var cookieOptions = new CookieOptions(); // nesne oluşturuk, instance aldık

      // çerez politikası
      var cookie = Request.Cookies["SPT"]; //tarayıcıda SPT isminde cookie var mı diye bakıyoruz
      if (cookie == null)
      {
        //kullanıcı sisteme ilk defa ürün ekleyecek
        cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTime.Now.AddDays(1); // 1 günlük çerez tanımladık (geriye dönük için - li değer)
        cookieOptions.Path = "/";
        order.Sepet = "";
        order.AddToCart(id.ToString()); //Sepete ekle metodu

        Response.Cookies.Append("SPT", order.Sepet, cookieOptions);//tanımladığımız çerezi tarayıcıya gönderdik
                                                                   //.Session.SetString("Message", "Ürün Sepetinize Eklendi");
                                                                   //TempData["Message"] = "Ürün Sepetinize Eklendi";
        message = "Ürün Sepetinize Eklendi";
      }
      else
      {
        order.Sepet = cookie; //kullanıcı daha önceden sepetine ürün eklemiş tarayıcıdaki sepetim içeriğini property'ye gönderiyoruz

        // aynı ürün daha önceden sepete eklenmiş mi
        if (order.AddToCart(id.ToString()) == false)
        {
          Response.Cookies.Append("SPT", order.Sepet, cookieOptions); //eklenmemiş
          cookieOptions.Expires = DateTime.Now.AddDays(1);
          // HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
          //TempData["Message"] = "Ürün Sepetinize Eklendi";
          message = "Ürün Sepetinize Eklendi";
        }
        else
        {
          // HttpContext.Session.SetString("Message", "Bu ürün Sepetinizde Zaten Var");
          //TempData["Message"] = "Bu ürün Sepetinizde Zaten Var";
          message = "Bu ürün Sepetinizde Zaten Var";

        }
      }
      // cookie = Request.Cookies["SPT"];
      //int itemCount = cookie != null ? cookie.Split('&').Length : 0;
      return Json(new { success = true, message = message });
    }
    public IActionResult CartSummary()
    {
      return ViewComponent("CardSummary");
    }
    public IActionResult Confirm()
    {
      ViewBag.OrderGroupGUID = OrderGroupGUID;
      ViewBag.cevap = cevap; //netgsm'den dönen cevap
      return View();
    }
    public IActionResult MyOrders()
    {
      if (HttpContext.Session.GetString("Email") != null)
      {
        List<MyOrdersViewModel> orders = order.SelectMyOrders(HttpContext.Session.GetString("Email")!.ToString());
        return View(orders);
      }
      return RedirectToAction("Login");
    }
    public async Task<IActionResult> DetailedSearch()
    {
      ViewBag.Categories = await category.GetCategoriesAsync();
      ViewBag.Supliers = await suplier.GetSuppliersAsync();
      return View();
    }
    public IActionResult DpProduct(int CategoryID, string[] SupplierID, string price, string isInStock)
    {
      price = price.Replace(" ", "").Replace("TL", "");//200 - 2000 arasındaki boşluk ve PArabirimi siliindi.

      string[] priceArray = price.Split("-");
      string startmoney = priceArray[0];
      string endmoney = priceArray[1];

      //string sign = ">";
      //if (isInStock == "0")
      //{
      //   sign = ">=";
      //}

      string SupplierValue = "";

      for (int i = 0; i < SupplierID.Length; i++)
      {
        if (i != 0)

          //select * from Products where (SupplierID=2 or SupplierID=3 or SupplierID=4)
          SupplierValue += " or ";

        SupplierValue += "SupplierID = " + SupplierID[i];
        //if (i==0)
        //   //select * from Products where (SupplierID = 2)
        //   SupplierValue = "SupplierID = " + SupplierID[i];
        //else
        //   //select * from Products where (SupplierID=2 or SupplierID=3 or SupplierID=4)
        //   SupplierValue += " or SupplierID= " + SupplierID[i];
      }
      if (SupplierValue != "")
      {
        SupplierValue = "(" + SupplierValue + ") and ";
      }

      string query = $"select * from Products where CategoryID= {CategoryID} and {SupplierValue} (UnitPrice >= {startmoney} and UnitPrice <= {endmoney}) and Stock >= {isInStock} order by AddDate Desc";
      ViewBag.Products = order.Select_Products_DetailsSearch(query);

      return View();
    }
    public PartialViewResult gettingProducts(string id)
    {
      id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
      List<QuickSearchViewModel> ulist = ProductService.gettingSearchProducts(id);
      //string json = JsonConvert.SerializeObject(ulist);
      //var response = JsonConvert.DeserializeObject<List<Search>>(json);
      return PartialView(ulist);
    }
    public IActionResult AboutUs()
    {
      return View();
    }
    [HttpPost]
    public IActionResult ContactUs(Message message)
    {
      context.Add(message);
      context.SaveChanges();
      ViewBag.Message = "Mesaj Gönderildi.";
      return View();  
    }
    [HttpGet]
    public IActionResult ContactUs()
    {
      return View();
    }








  }
}
