const connection = new signalR.HubConnectionBuilder().withUrl("/adminHub").build();
connection.on("KullaniciHareketiniYakala", function (kullanici, aktivite) {
  const mesaj = `${kullanici} kullanıcısı şunu yaptı:${aktivite}`
  const li = document.createElement("li");
  li.textContent = mesaj;
  document.getElementById("hareketListesi").appendChild(li);
});
connection.start().catch(function (err) {
  return console.error(err.ToString());
});