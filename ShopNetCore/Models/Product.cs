using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopNetCore.Models
{
  public class Product
  {

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DisplayName("ID")]
    public int ProductID { get; set; }

    [Required(ErrorMessage = "Ürün Adı Boş Geçilemez")]
    [StringLength(100, ErrorMessage = "50 karakterden uzun olamaz")]
    [DisplayName("Ürün Adı")]
    public string? ProductName { get; set; }

    [DisplayName("Fiyat")]
    public decimal UnitPrice { get; set; }
    [DisplayName("Kategori")]
    public int CategoryID { get; set; }

    [DisplayName("Marka")]
    public int SupplierID { get; set; }
    [DisplayName("Stok")]
    public int Stock { get; set; }
    [DisplayName("İndirim")]
    public int Discount { get; set; }
    [DisplayName("Durum")]
    public int StatusID { get; set; }

    [DisplayName("Eklenme Tarihi")]
    public DateTime AddDate { get; set; }
    [DisplayName("Anahtar Kelimeler")]
    public string? Keywords { get; set; }

    private int _Kdv { get; set; }
    [DisplayName("KDV")]
    public int Kdv
    {
      get { return _Kdv; }
      set { _Kdv = Math.Abs(value); }
    }

    public int HighLighted { get; set; }  // Öne Çıkanlar

    public int TopSeller { get; set; }    // Çok Satanlar



    [DisplayName("İlişkili")]
    public int Related { get; set; }     // Buna Bakanlar


    [DisplayName("Notlar")]
    public string? Notes { get; set; }
    [DisplayName("Fotoğraf")]
    public string? PhotoPath { get; set; }
    [DisplayName("Aktif/Deaktif")]

    public bool Active { get; set; }

    public int ToplamPuan { get; set; }
    public int OySayisi { get; set; }
    public decimal OrtalamaPuan { get; set; }

  }
}
