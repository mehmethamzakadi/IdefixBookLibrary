using IdefixKitapLibrary.Database;
using IdefixKitapLibrary.Models;
using System.Text.Json;

using (HttpClient client = new HttpClient())
{
    try
    {
        // İstekte bulunmak istediğiniz URL
        string url = $"https://www.idefix.com/_next/data/DZc9ty2rWkeNzKyJB1EKG/kitap-kultur-c-3307.json?isSalable=false&slug=kitap-kultur-c-3307&siralama=desc_adde&sayfa=1";

        // GET isteği yapın
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        // Cevap içeriğini string olarak alın
        string responseBody = await response.Content.ReadAsStringAsync();

        // JSON formatında ayrıştırma
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var issues = JsonSerializer.Deserialize<IdefixBookResult>(responseBody, options);
        var totalPage = issues.PageProps.CategoryData.RecordCount;

        for (int i = 296; i < totalPage; i++)
        {
            string url2 = $"https://www.idefix.com/_next/data/DZc9ty2rWkeNzKyJB1EKG/kitap-kultur-c-3307.json?isSalable=false&slug=kitap-kultur-c-3307&siralama=desc_adde&sayfa={i}";

            await Task.Delay(10000); // 10 saniye bekler

            HttpResponseMessage response2 = await client.GetAsync(url2);

            while (response2.StatusCode != System.Net.HttpStatusCode.OK)
            {
                response2 = await Refresh(i, client);
            }

            response2.EnsureSuccessStatusCode();



            // Cevap içeriğini string olarak alın
            string responseBody2 = await response2.Content.ReadAsStringAsync();

            // JSON formatında ayrıştırma
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var issues2 = JsonSerializer.Deserialize<IdefixBookResult>(responseBody2, options2);


            var kitapList = new List<Kitap>();
            foreach (var item in issues2.PageProps.CategoryData.Items)
            {
                string kitapAdi = "";
                string[] parts = item.Name.Split(new string[] { " - " }, StringSplitOptions.None);
                if (parts[0] == "İmzalı")
                    kitapAdi = parts[1];
                else
                    kitapAdi = parts[0];

                string fullName = item.Name;
                string kategoriAdi = item.Properties.FirstOrDefault(x => x.Text == "Kategori")?.ValueText;
                string basimDili = item.Properties.FirstOrDefault(x => x.Text == "Basım Dili")?.ValueText;

                var kitap = new Kitap
                {
                    IdefixId = item.Id,
                    KitapAdi = kitapAdi,
                    FullName = fullName,
                    BasimDili = basimDili,
                    Kategori = kategoriAdi,
                    YayinEvi = item.BrandName,
                    YazarAdi = item.AuthorName,
                    ImageUrl = item.Images.Count > 0 ? item.Images[0]?.Src : "",
                    ImageHeight = item.Images.Count > 0 ? item.Images[0].Height : 0,
                    ImageWidth = item.Images.Count > 0 ? item.Images[0].Height : 0,
                };
                kitapList.Add(kitap);
            }

            using var context = new BookLibraryContext();
            await context.Kitaplar.AddRangeAsync(kitapList);
            await context.SaveChangesAsync();

            Console.WriteLine($"{i} Nolu Sayfa Eklendi.");
        }
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine("\nException Caught!");
        Console.WriteLine("Message :{0} ", e.Message);
    }
}

async Task<HttpResponseMessage> Refresh(int sayfa, HttpClient client)
{
    string url2 = $"https://www.idefix.com/_next/data/DZc9ty2rWkeNzKyJB1EKG/kitap-kultur-c-3307.json?isSalable=false&slug=kitap-kultur-c-3307&siralama=desc_adde&sayfa={sayfa}";

    await Task.Delay(5000); // 10 saniye bekler

    Console.WriteLine($"{sayfa} Nolu Sayfa Yeniden istek atıldı.");

    HttpResponseMessage response2 = await client.GetAsync(url2);

    return response2;
}
