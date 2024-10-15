using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopNetCore.Models
{
  public class User
  {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    [StringLength(100)]
    public string? NameSurname { get; set; }

    [EmailAddress]

    [StringLength(100)]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    public string? Password { get; set; }


    public string? Telephone { get; set; }

    public string? InvoicesAddress { get; set; }


    public bool IsAdmin { get; set; }

    public bool Active { get; set; }














  }
}
