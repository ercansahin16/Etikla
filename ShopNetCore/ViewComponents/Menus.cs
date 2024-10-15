using Microsoft.AspNetCore.Mvc;
using ShopNetCore.Data;
using ShopNetCore.Models;

namespace ShopNetCore.ViewComponents
{
  public class Menus : ViewComponent
  {
    ShopNetCoreContext context = new();

    public IViewComponentResult Invoke()
    {
      List<Category> categories = context.Categories.Where(x => x.Active == true).ToList();
      return View(categories);
    }













  }
}
