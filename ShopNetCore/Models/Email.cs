using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopNetCore.Models
{
  public class Email
  {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmailID { get; set; }

    public string? MailAdres { get; set; }

    public bool Active { get; set; }





  }
}
