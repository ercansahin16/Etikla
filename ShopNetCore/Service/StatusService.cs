using Microsoft.EntityFrameworkCore;
using ShopNetCore.Data;
using ShopNetCore.Models;

namespace ShopNetCore.Service
{
  public class StatusService
  {
    ShopNetCoreContext context = new();

    public async Task<List<Status>> GetStatusAsync()
    {
      List<Status> statusList = await context.Statuses.ToListAsync();
      return statusList;
    }

    public static bool StatusInsert(Status status)
    {
      using (ShopNetCoreContext context = new())
      {

        try
        {
          context.Add(status);
          context.SaveChanges();
          return true;
        }
        catch (Exception)
        {

          return false;
        }
      }

    }
    public async Task<Status?> GetStatusDetailsAsync(int? id)
    {
      Status? status = await context.Statuses.FirstOrDefaultAsync(st => st.StatusID == id);
      return status;
    }


    public static bool StatusUpdate(Status status)
    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          context.Update(status);
          context.SaveChanges();
          return true;
        }
        catch (Exception)
        {

          return false;
        }

      }
    }


    public static bool StatusDeleteConfirmed(int id)
    {
      using (ShopNetCoreContext context = new())
      {
        try
        {
          Status? status = context.Statuses.FirstOrDefault(st => st.StatusID == id);
          status!.Active = false;
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
