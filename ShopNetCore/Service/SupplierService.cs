using Microsoft.EntityFrameworkCore;
using ShopNetCore.Data;
using ShopNetCore.Models;

namespace ShopNetCore.Service
{
  public class SupplierService
  {
    ShopNetCoreContext context = new();
    public async Task<List<Supplier>> GetSupplierAsync()
    {
      List<Supplier> suppliers = await context.Suppliers.ToListAsync();
      return suppliers;

    }
    public static bool SupplierInsert(Supplier supplier)
    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          context.Add(supplier);
          context.SaveChanges();
          return true;
        }
        catch (Exception)
        {

          return false;

        }
      }
    }
    public async Task<Supplier?> GetSupplierDetailsAsync(int? id)
    {
      Supplier? supplier = await context
          .Suppliers
          .FirstOrDefaultAsync(s => s.SupplierID == id);
      return supplier;
    }
    public static bool SupplierUpdate(Supplier supplier)
    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          context.Update(supplier);
          context.SaveChanges();
          return true;
        }
        catch (Exception)
        {

          return false;
        }
      }
    }
    public static bool SupplierDeleteConfirmed(int id)
    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          Supplier? supplier = context.Suppliers.FirstOrDefault(s => s.SupplierID == id);
          supplier!.Active = false;
          context.SaveChanges();
          return true;

        }
        catch (Exception)
        {
          return false;

        }
      }
    }
    public async Task<List<Supplier>> GetSuppliersAsync()
    {
      List<Supplier> suppliers = await context.Suppliers.ToListAsync();//Bütün kategorileri liste olarak döner(asenkron)
      return suppliers;
    }








  }
}
