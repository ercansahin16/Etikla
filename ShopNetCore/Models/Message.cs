using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopNetCore.Models
{
  public class Message
  {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageID { get; set; }
    public string? NameSurname { get; set; }
    public string? Mail { get; set; }
    public string? Subject { get; set; }
    public string? Content { get; set; }












  }
}
