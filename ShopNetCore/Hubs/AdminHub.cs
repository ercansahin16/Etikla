using Microsoft.AspNetCore.SignalR;

namespace ShopNetCore.Hubs
{
  public class AdminHub : Hub
  {
    // buraya methodlar gelecek

    public async Task KullaniciHareketiGonder(string kullanici, string aktivite)
    {
      await Clients.All.SendAsync("KullaniciHareketiniYakala", kullanici, aktivite);
    }










  }
}
