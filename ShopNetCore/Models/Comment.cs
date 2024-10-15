using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopNetCore.Models
{
  public class Comment
  {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CommentID { get; set; }
    public int UserID { get; set; }
    public int ProductID { get; set; }
    [StringLength(250, ErrorMessage = "150 karakterden fazla olamaz")]
    public string? Review { get; set; }
    public DateTime AddDate { get; set; }
    public int Puan {  get; set; }




















  }
}
