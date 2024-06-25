using IdefixKitapLibrary.Database;
using IdefixKitapLibrary.Islemler;

using var context = new BookLibraryContext();
using var client = new HttpClient();
client.Timeout = TimeSpan.FromMinutes(5);

while (true)
{
    Console.WriteLine("Hangi işlemi yapmak istiyorsunuz?");
    Console.WriteLine("1. Idefix Üzerindeki Tüm Kitapları Çek ve Kaydet. (Son eklenenlere göre sıralı)");
    Console.WriteLine("2. Yeni Eklenen Kitapları Çek ve Kaydet");
    Console.WriteLine("5. Çıkış");
    Console.Write("Seçiminiz: ");

    string secim = Console.ReadLine();

    switch (secim)
    {
        case "1":
            await Idefix.TumunuCek();
            break;
        case "2":
            await Idefix.YeniEklenenKitaplariCek();
            break;
        case "3":
            break;
        case "4":
            break;
        case "5":
            Console.WriteLine("Çıkış yapılıyor...");
            return;
        default:
            Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
            break;
    }

    Console.WriteLine();
}