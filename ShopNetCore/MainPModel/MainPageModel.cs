using ShopNetCore.Models;

namespace ShopNetCore.MainPModel
{
  public class MainPageModel
  {
    public List<Product>? SliderProducts { get; set; }        //Slider               StatusID = 1

    public List<Product>? NewProducts { get; set; }           //Yeni ürün            AddDate Kolonu
    public Product? ProductOfDay { get; set; }               //Günün ürünü           StatusID = 6
    public List<Product>? SpecialProducts { get; set; }      //Özel                  StatusID = 2
    public List<Product>? StarProducts { get; set; }         //Yıldızlı              StatusID = 3
    public List<Product>? FeaturedProducts { get; set; }     // Fırsat               StatusID = 4
    public List<Product>? DiscountedProducts { get; set; }   // indirimli            Discount Kolonu
    public List<Product>? HighlightedProducts { get; set; }  // Öne Çıkanlar         Highlighted kolonu
    public List<Product>? TopSellerProducts { get; set; }   // Çok Satanlar           TopSeller Kolonu
    public List<Product>? NotableProducts { get; set; }     // Dikkat Çeken             StatusID = 5

    public List<Product>? ProductsByCategory { get; set; }  // Kategorisine göre ürünler (CategoryPage) 
    public Product? ProductDetails { get; set; }

    public string? CategoryName { get; set; }

    public string? BrandName { get; set; }

    public List<Product>? RelatedProducts { get; set; }

    public List<Comment>? Comments { get; set; }









  }
}
