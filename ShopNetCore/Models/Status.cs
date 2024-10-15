using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopNetCore.Models
{
  public class Status
  {

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StatusID { get; set; }

    [Required(ErrorMessage = "Statu Adı Boş Geçilemez")]
    [StringLength(100, ErrorMessage = "Statu Adı 100 Karakterden büyük olamaz")]
    [DisplayName("Durum Adı")]

    public string? StatusName { get; set; }
    [DisplayName("Aktif/Deaktif")]
    public bool Active { get; set; }












  }
}
