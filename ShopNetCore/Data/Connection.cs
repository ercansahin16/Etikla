using Microsoft.Data.SqlClient;

namespace ShopNetCore.Data
{
  public class Connection
  {
    public static SqlConnection baglanti
    {
      get
      {
        SqlConnection sqlcon = new SqlConnection("Server=DESKTOP-EGM53VS\\SQLAYTU; Database=ShopNetDB; Trusted_Connection=true; TrustServerCertificate=True;");
        return sqlcon;
      }
    }







  }
}
