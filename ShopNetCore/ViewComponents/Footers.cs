using Microsoft.AspNetCore.Mvc;
using ShopNetCore.Data;
using ShopNetCore.Models;

namespace ShopNetCore.ViewComponents
{
  public class Footers : ViewComponent
  {
    ShopNetCoreContext context = new();

    public IViewComponentResult Invoke()
    {
      List<Supplier> suppliers = context.Suppliers.Where(x => x.Active == true).ToList();
      return View(suppliers);
    }

  }
}
