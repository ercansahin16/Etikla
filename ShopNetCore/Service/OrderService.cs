using Microsoft.Data.SqlClient;
using ShopNetCore.Data;
using ShopNetCore.Models;
using ShopNetCore.ViewModels;

namespace ShopNetCore.Service
{
  public class OrderService
  {
    public int ProductID { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public string? Sepet { get; set; }
    public decimal UnitPrice { get; set; }
    public int Kdv { get; set; }
    public string? PhotoPath { get; set; }
    ShopNetCoreContext context = new();
    //SEPETE EKLE 
    public bool AddToCart(string id)
    {
      bool isExits = false;

      if (Sepet == "")
      {
        //sepete ilk defa ürün ekliyor
        Sepet = id + "=1";

      }
      else
      {
        // daha önceden sepete birşeyler eklemiş şuan eklemek istediği şey sepetinde var mı bilmiyoruz kontrol etmeliyiz

        string[] basketArray = Sepet!.Split('&');
        for (int i = 0; i < basketArray.Length; i++)
        {
          // 10=1 -> sepetdizi[0]
          // 20=1 -> sepetdizi[1]

          string[] basketArray2 = basketArray[i].Split('=');

          if (basketArray2[0] == id)
          {
            // bu ürün sepette zaten var
            isExits = true;
          }

        }

        if (isExits == false)
        {
          // ürün sepete daha önce eklenmemiş
          Sepet = Sepet + "&" + id + "=1";
        }

      }
      return isExits;
    }
    // projede sağ üst köşedeki sepet sayfası ve sil butonu tıklanınca yüklenecek olan sayfa bu metodu çağıracak
    //List<cls_Order> = propertyleri dönecek
    // siparişi onaylama metodu da çağırıyor
    public List<OrderService> GetMyCart()
    {
      List<OrderService> list = new List<OrderService>();
      string[] basketArray = Sepet!.Split('&'); // Sepette ki ürünleri ayırdık
      if (basketArray[0] != "")
      {
        for (int i = 0; i < basketArray.Length; i++)
        {
          string[] basketArray2 = basketArray[i].Split('=');
          int basketId = Convert.ToInt32(basketArray2[0]); //ProductID yi alıyoruz
          int adet = Convert.ToInt32(basketArray2[1]); //quantity 

          Product? product = context.Products.FirstOrDefault(p => p.ProductID == basketId);

          OrderService p = new OrderService();
          p.ProductID = product!.ProductID;
          p.ProductName = product.ProductName;
          p.Quantity = adet;
          p.UnitPrice = product.UnitPrice;
          p.Kdv = product.Kdv;
          p.PhotoPath = product.PhotoPath;
          list.Add(p);
        }
      }
      return list;
    }
    public void DeleteFromMyCart(string id)
    {
      string[] basketArray = Sepet!.Split('&'); //ürünleri ayırıyoruz
      string newBasket = "";
      for (int i = 0; i < basketArray.Length; i++)
      {
        string[] basketArray2 = basketArray[i].Split('=');
        int adet = Convert.ToInt32(basketArray2[1]);
        if (basketArray2[0] != id)
        {
          // silinmeyecek id'li ürünleri burada yakaladık ve yeni sepet oluşturuyoruz
          if (newBasket == "")
          {
            newBasket = basketArray2[0] + "=" + adet.ToString();
          }
          else
          {
            //  newBasket += basketArray2[0] + "&" + adet.ToString();
            newBasket = newBasket + "&" + basketArray2[0] + "=" + adet.ToString();
          }
        }
      }
      Sepet = newBasket;
    }

    public string WriteFromCookieToTable(string Email)
    {
      // ürünleri ayrı ayrı dönerken (siparişler tablosuna işlerken)
      // o ürünlerin stok değerlerini quantity kadar azalt
      // ve yine o ürünlerin topseller kolonunu quantity kadar arttır

      string OrderGroupGUID = DateTime.Now.ToString().Replace(".", "").Replace(":", "").Replace(" ", "");
      DateTime orderDate = DateTime.Now;



      List<OrderService> orders = GetMyCart();
      foreach (var item in orders)
      {
        Order order = new();
        order.OrderDate = orderDate;
        order.OrderGroupGUID = OrderGroupGUID;
        order.UserID = context.Users.FirstOrDefault(u => u.Email == Email)!.UserID;
        order.ProductID = item.ProductID;
        order.Quantity = item.Quantity;
        context.Orders.Add(order);

        Product product = context.Products.FirstOrDefault(u => u.ProductID == order.ProductID)!;
        product!.Stock = product.Stock - order.Quantity;

        if (product.Stock == 0)
          product.Active = false;

        product.TopSeller = product.TopSeller + order.Quantity;
        context.SaveChanges();
      }
      return OrderGroupGUID;
    }

    public List<MyOrdersViewModel> SelectMyOrders(string Email)
    {
      int userID = context.Users.FirstOrDefault(x => x.Email == Email)!.UserID;
      List<MyOrdersViewModel> orders = context.vw_MyOrders.Where(x => x.UserID == userID).ToList();
      return orders;
    }
    public List<OrderService> Select_Products_DetailsSearch(string query)
    {
      List<OrderService> products = new();

      SqlConnection sqlcon = Connection.baglanti;
      SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
      sqlcon.Open();
      SqlDataReader sqlDataReader = sqlcmd.ExecuteReader();
      while (sqlDataReader.Read())
      {
        OrderService p = new OrderService();
        p.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
        p.ProductName = sqlDataReader["ProductName"].ToString();
        p.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
        p.PhotoPath = sqlDataReader["PhotoPath"].ToString();
        products.Add(p);
      }
      sqlcon.Close();
      return products;
    }









  }
}
