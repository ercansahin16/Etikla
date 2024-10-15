using Microsoft.EntityFrameworkCore;
using ShopNetCore.Data;
using ShopNetCore.Models;
using ShopNetCore.ViewModels;
using System.Data;

namespace ShopNetCore.Service
{

  public class ProductService
  {
    ShopNetCoreContext context = new();

    public async Task<List<Product>> GetProductsAsync()
    {
      List<Product> products = await context.Products.ToListAsync();
      return products;
    }

    public static bool ProductInsert(Product product)
    {
      //metod static olduğu için context direkt gelmez
      using (ShopNetCoreContext context = new())
      {
        try
        {
          if (product.Notes == null)
            product.Notes = "";

          product.AddDate = DateTime.Now;

          bool exist = context.
               Products.
               Any(p => p
              .ProductName!.ToLower().Trim() == product.ProductName!.ToLower().Trim()); //equals(product.ProductName)'de kullanılabilir

          if (!exist)
          {
            context.Add(product);
            context.SaveChanges();
            return true;
          }

          return false;

        }
        catch (Exception)
        {

          return false;
        }
      }
    }

    public async Task<Product?> GetProductsDetailsAsync(int? id)
    {
      Product? product = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
      return product;
    }

    public static bool ProductUpdate(Product product)

    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          context.Update(product);
          context.SaveChanges();
          return true;
        }
        catch (Exception)
        {

          return false;
        }
      }
    }

    public static bool ProductDeleteConfirmed(int id)
    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          Product? product = context.Products.FirstOrDefault(p => p.ProductID == id);
          product!.Active = false;
          context.SaveChanges();
          return true;
        }
        catch (Exception)
        {

          return false;
        }
      }
    }

    public List<Product> GetProducts(string mainPageName, string subPageName, int pagenumber = 0)
    {

      List<Product> products;

      int? mainPageCount = context.Settings.FirstOrDefault(x => x.SettingID == 1)?.MainpageCount;
      int? subPageCount = context.Settings.FirstOrDefault(x => x.SettingID == 1)?.SubpageCount;

      
      if (mainPageName == "SliderProducts")
      {
        // Slider Ürünleri Getiren Sorgu
        products = context.Products.Where(x => x.StatusID == 1 && x.Active == true).Take(Convert.ToInt32(mainPageCount)).ToList();
      }
      else if (mainPageName == "NewProducts")
      {
        if (subPageName == "index")
        {
          // Home/Index
          products = context.Products.Where(x => x.Active == true).OrderByDescending(p => p.AddDate).Take(Convert.ToInt32(mainPageCount)).ToList();
        }
        else
        {
          if (subPageName == "topmenu")
          {
            //menüden new tıklanınca
            products = context.Products.Where(x => x.Active == true).OrderByDescending(p => p.AddDate).Take(Convert.ToInt32(subPageCount)).ToList();
          }
          else
          {
            //ajax
            products = context.Products.Where(x => x.Active == true).OrderByDescending(p => p.AddDate).Skip(pagenumber * Convert.ToInt32(subPageCount)).Take(Convert.ToInt32(subPageCount)).ToList();
          }
        }
      }
      else if (mainPageName == "SpecialProducts")
      {
        //özel
        if (subPageName == "index")
        {
          // Home/SpecialProducts
          products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).OrderByDescending(o => o.AddDate).Take(Convert.ToInt32(mainPageCount)).ToList();
        }
        else
        {
          if (subPageName == "topmenu")
          {
            //menüden özel tıklanınca
            products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).OrderByDescending(o => o.AddDate).Take(Convert.ToInt32(subPageCount)).ToList();
          }
          else
          {
            //ajax
            products = context.Products.Where(p => p.StatusID == 2 && p.Active == true)
               .OrderByDescending(o => o.AddDate)
               .Skip(pagenumber * Convert.ToInt32(subPageCount)).Take(Convert.ToInt32(subPageCount)).ToList();
          }
        }
      }

      else if (mainPageName == "DiscountedProducts")
      {
        //İndirimli
        if (subPageName == "index")
        {
          // Home/DiscountedProducts
          products = context.Products.Where(x => x.Active == true)
             .OrderByDescending(p => p.Discount)
             .Take(Convert.ToInt32(mainPageCount)).ToList();
        }
        else
        {
          if (subPageName == "topmenu")
          {
            //menüden İndirimli tıklanınca
            products = context.Products.Where(x => x.Active == true)
               .OrderByDescending(p => p.Discount)
               .Take(Convert.ToInt32(subPageCount)).ToList();
          }
          else
          {
            //ajax
            products = context.Products.Where(x => x.Active == true)
               .OrderByDescending(p => p.Discount)
               .Skip(pagenumber * Convert.ToInt32(subPageCount)).Take(Convert.ToInt32(subPageCount)).ToList();
          }
        }
      }
      else if (mainPageName == "HighlightedProducts")
      {
        // Öne Çıkan
        if (subPageName == "index")
        {
          // Home/HighlightedProducts
          products = context.Products.Where(x => x.Active == true).OrderByDescending(p => p.HighLighted).Take(Convert.ToInt32(mainPageCount)).ToList();
        }
        else
        {
          if (subPageName == "topmenu")
          {
            //menüden öne çıkan tıklanınca
            products = context.Products.Where(x => x.Active == true)
               .OrderByDescending(p => p.HighLighted)
               .Take(Convert.ToInt32(subPageCount)).ToList();
          }
          else
          {
            //ajax
            products = context.Products.Where(x => x.Active == true)
               .OrderByDescending(p => p.HighLighted).Skip(pagenumber * Convert.ToInt32(subPageCount))
               .Take(Convert.ToInt32(subPageCount)).ToList();
          }
        }
      }








      else if (mainPageName == "StarProducts")
      {
        // Yıldız 
        products = context.Products.Where(p => p.StatusID == 3 && p.Active == true).OrderByDescending(o => o.AddDate).Take(Convert.ToInt32(mainPageCount)).ToList();
      }
      else if (mainPageName == "FeaturedProducts")
      {
        // Firsat 
        products = context.Products.Where(p => p.StatusID == 4 && p.Active == true).OrderByDescending(o => o.AddDate).Take(Convert.ToInt32(mainPageCount)).ToList();
      }

      else if (mainPageName == "TopSellerProducts")
      {
        // Çok satan 
        products = context.Products.Where(x => x.Active == true).OrderByDescending(p => p.TopSeller).Take(Convert.ToInt32(mainPageCount)).ToList();
      }
      else if (mainPageName == "NotableProducts")
      {
        // Dikkat 
        products = context.Products.Where(x => x.StatusID == 5 && x.Active == true).Take(Convert.ToInt32(mainPageCount)).ToList();
      }

      else
        return null;

      return products;

    }

    public Product? GetProductOfDay()
    {
      //Günün Ürününü getiren sorgu
      Product? product = context.Products.FirstOrDefault(x => x.StatusID == 6);

      return product;

    }

    public List<Product> GetProductsByCategory(int id)
    {
      List<Product> products = context.Products
        .Where(x => x.CategoryID == id && x.Active == true)
        .OrderByDescending(x => x.AddDate).ToList();
      return products;
    }

    public List<Product> GetProductsBySupplier(int id)
    {
      List<Product> products = context.Products
        .Where(x => x.SupplierID == id && x.Active == true)
        .OrderByDescending(x => x.AddDate).ToList();
      return products;
    }

    public void HighlightedPlus(int id)
    {
      // 1. Products tablosundan ürünü bul
      // 2. bulduğun ürünün Highlighted kolonunu 1 arttır
      using (ShopNetCoreContext context = new())
      {
        Product? product = context.Products.FirstOrDefault(p => p.ProductID == id);
        product!.HighLighted = product.HighLighted + 1;
        context.Update(product);
        context.SaveChanges();
      }

    }


    public static List<QuickSearchViewModel> gettingSearchProducts(string id)
    {
      using (ShopNetCoreContext context = new())
      {
        var products = context.sp_Aramas.FromSql($"sp_arama {id}").ToList();
        return products;
      }
    }












  }
}
