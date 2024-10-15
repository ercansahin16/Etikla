using Microsoft.AspNetCore.Mvc;

namespace ShopNetCore.ViewComponents
{
  public class CardSummary : ViewComponent
  {
    public IViewComponentResult Invoke()
    {
      var cookie = Request.Cookies["SPT"];
      int itemCount = cookie != null ? cookie.Split('&').Length : 0;
      return View(itemCount);
    }














  }
}
