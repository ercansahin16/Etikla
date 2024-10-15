using Microsoft.EntityFrameworkCore;
using ShopNetCore.Data;
using ShopNetCore.Models;

namespace ShopNetCore.Service
{
  public class CategoryService
  {
    ShopNetCoreContext context = new();
    // Bütün Kategorileri Liste Olarak döner (Asenkron)
    public async Task<List<Category>> GetCategoriesAsync()
    {
      List<Category> categories = await context.Categories.ToListAsync();
      return categories;
    }
    //Main Kategorileri liste olarak döner
    public List<Category> GetMainCategories()
    {
      List<Category> categories = context.Categories.Where(x => x.ParentID == 0).ToList();
      return categories;
    }

    public static bool CategoryInsert(Category category)
    {
      // Method static olduğu için context direk gelmez
      using (ShopNetCoreContext ctx = new())
      {
        try
        {
          ctx.Add(category);
          ctx.SaveChanges();
          return true;
        }
        catch (Exception)
        {

          return false;
        }

      }

    }


    public async Task<Category?> GetCategoryDetailsAsync(int? id)
    {
      Category? category = await
          context
          .Categories
          .FirstOrDefaultAsync(x => x.CategoryID == id);
      return category;
    }

    public static bool CategoryUpdate(Category category)
    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          context.Update(category);
          context.SaveChanges();
          return true;
        }
        catch (Exception)
        {

          return false;
        }
      }
    }

    public static bool CategoryDelete(int id)
    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          Category? category = context
              .Categories

              .FirstOrDefault(x => x.CategoryID == id);
          category!.Active = false;


          List<Category> categories = context
              .Categories
              .Where(x => x.ParentID == id).ToList();

          foreach (var x in categories)
          {
            x.Active = false;
          }

          context.SaveChanges();
          return true;

        }
        catch (Exception)
        {

          return false;
        }
      }
    }














  }
}
