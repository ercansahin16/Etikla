using Microsoft.EntityFrameworkCore;
using ShopNetCore.Models;
using ShopNetCore.ViewModels;

namespace ShopNetCore.Data
{
  public class ShopNetCoreContext : DbContext
  {

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
      var configuration = builder.Build();
      optionsBuilder.UseSqlServer(configuration["ConnectionStrings:ShopNetConnection"]);
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Email> Emails { get; set; }


    // aşağıdakiler tablo değil
    public DbSet<MyOrdersViewModel> vw_MyOrders { get; set; }
    public DbSet<QuickSearchViewModel> sp_Aramas { get; set; }










  }
}
