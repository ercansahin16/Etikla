using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopNetCore.Models
{
  public class Category
  {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryID { get; set; }
    public int ParentID { get; set; }

    [StringLength(50, ErrorMessage = "En fazla 50 karakter olabilir")]
    [Required(ErrorMessage = "Kategori Adı Boş Bırakılamaz")]
    [DisplayName("Kategori Adı")]
    public string? CategoryName { get; set; }
    [DisplayName("Aktif/Deaktif")]

    public bool Active { get; set; }




  }












}
