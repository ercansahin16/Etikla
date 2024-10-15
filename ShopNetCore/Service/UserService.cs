using Microsoft.EntityFrameworkCore;
using ShopNetCore.Data;
using ShopNetCore.Models;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace ShopNetCore.Service
{
  public class UserService
  {
    ShopNetCoreContext context = new();
    public async Task<User?> loginControl(User user)
    {
      var value = MD5Sifrele(user.Password);
      User? usr = await context.Users.
        FirstOrDefaultAsync(x =>
          x.Email == user.Email &&
          x.Password == value &&
          x.IsAdmin == true &&
          x.Active == true);
      return usr;
    }
    public static User? GetUserInfo(string Email)
    {
      using (ShopNetCoreContext context = new())
      {
        User? user = context.Users.FirstOrDefault(x => x.Email == Email);
        return user;
      }



    }

    //  string answer = cls_User.AddUser(user);
    public static string AddUser(User user)
    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email);
          if (usr != null)
          {
            // bu mail daha önceden kayıtlı
            return "Email zaten var";
          }
          else
          {
            user.Active = true;
            user.IsAdmin = false;
            user.Password = MD5Sifrele(user.Password);
            context.Users.Add(user);
            context.SaveChanges();
            return "Başarılı";
          }
        }
        catch (Exception)
        {
          return "Başarısız";
        }
      }
    }
    public static string MD5Sifrele(string value)
    {
      using (MD5 md5 = MD5.Create())
      {
        byte[] btr = Encoding.UTF8.GetBytes(value);
        btr = md5.ComputeHash(btr);

        StringBuilder sb = new();
        foreach (byte item in btr)
        {
          sb.Append(item.ToString("x2").ToLower());
        }
        return sb.ToString();
      }
    }
    public static string SHA256Sifrele(string password)
    {
      using (SHA256 sha256Hash = SHA256.Create())
      {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder builder = new();
        foreach (byte item in bytes)
        {
          builder.Append(item.ToString("x2"));
        }
        return builder.ToString();
      }
    }
    public static string UserControl(User user)
    {
      using (ShopNetCoreContext context = new())
      {
        string answer = "";
        try
        {
          string md5sifrele = MD5Sifrele(user.Password);
          User? usr = context.Users
            .FirstOrDefault
            (x => x.Email == user.Email && x.Password == md5sifrele && x.Active == true);
          if (usr == null)
          {
            // email veya şifre yanlıştır (her ikiside yanlıştır) ya da aktif değil
            answer = "yanlış";
          }
          else
          {
            // email ve şifre doğru aynı zamanda aktif
            // admin mi normal kullanıcı mı
            if (usr.IsAdmin)
            {
              answer = "admin";
            }
            else
            {
              answer = usr.Email;
            }
          }
        }
        catch (Exception)
        {

          answer = "HATA";
        }
        return answer;








      }
    }
    public static string Send_Sms(string OrderrGroupGUID)
    {
      Order? order;
      User? user;
      string content;
      string? telefon;

      using (ShopNetCoreContext context = new())
      {
        order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderrGroupGUID);

        user = context.Users.FirstOrDefault(u => u.UserID == order.UserID);

        //Sayın Recep Şamil Çiftçi, 29.08.2024 tarihinde 29082024215730 nolu siparişiniz alınmıstır

        content = $"Sayın {user?.NameSurname}, {order?.OrderDate} tarihinde {OrderrGroupGUID} nolu siparişiniz alınmıstır";

        telefon = user?.Telephone;
      }

      string ss = "";
      ss += "<?xml version='1.0' encoding='UTF-8'?>";
      ss += "<mainbody>";
      ss += "<header>";
      ss += "<company dil='TR'>Netgsm</company>";
      ss += "<usercode>8503096835</usercode>";
      ss += "<password>738FC_B</password>";
      ss += "<type>1:n</type>";
      ss += "<msgheader>8503096835</msgheader>";
      ss += "</header>";
      ss += "<body>";
      ss += $"<msg><![CDATA[{content}]]></msg><no>0{telefon}</no>";
      ss += "</body>";
      ss += "</mainbody>";

      return XMLPOST("https://api.netgsm.com.tr/sms/send/xml", ss);
    }
    static string XMLPOST(string PostAddress, string xmlData)
    {
      try
      {
        WebClient wUpload = new WebClient();
        HttpWebRequest request = WebRequest.Create(PostAddress) as HttpWebRequest;
        request!.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
        Byte[] bResponse = wUpload.UploadData(PostAddress, "POST", bPostArray);
        Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
        string sWebPage = new string(sReturnChars);
        return sWebPage;
      }
      catch
      {
        return "-1";
      }
    }
    public static void Send_Email(string OrderrGroupGUID)
    {
      using (ShopNetCoreContext context = new())
      {
        Order order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderrGroupGUID)!;
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("no-reply@iamcodingaround.com", "ETicaret Bilgi");

        string subject = "Siparişiniz Hk.";

        User user = context.Users.FirstOrDefault(x => x.UserID == order.UserID)!;

        mailMessage.To.Add(user.Email!);
        string content = $"Sayın {user.NameSurname}, {order.OrderDate} tarihinde {OrderrGroupGUID} nolu siparişiniz alınmıştır.";

        mailMessage.Body = content;

        mailMessage.Subject = subject;
        SmtpClient smtp = new();
        smtp.Credentials = new NetworkCredential("no-reply@iamcodingaround.com", "8rh.I897t=B@fTZ.");
        smtp.Port = 587;
        smtp.Host = "mail.kurumsaleposta.com";
        try
        {
          smtp.Send(mailMessage);
        }
        catch (Exception)
        {

          // email gönderilemedi , ilgili personeli bilgilendir

          //no-reply@iamcodingaround.com
        }

      }
    }

   














  }
}
