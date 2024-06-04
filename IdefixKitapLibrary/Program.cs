using IdefixKitapLibrary;
using System.Text.Json;

using (HttpClient client = new HttpClient())
{
    try
    {
        // İstekte bulunmak istediğiniz URL
        string url = $"https://www.idefix.com/_next/data/DZc9ty2rWkeNzKyJB1EKG/kitap-kultur-c-3307.json?sayfa=1&slug=kitap-kultur-c-3307";

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
        var totalPage = issues.PageProps.CategoryData.Pages.Max();

        for (int i = 1; i < totalPage; i++)
        {
            string url2 = $"https://www.idefix.com/_next/data/DZc9ty2rWkeNzKyJB1EKG/kitap-kultur-c-3307.json?sayfa={i}&slug=kitap-kultur-c-3307";
            // GET isteği yapın
            HttpResponseMessage response2 = await client.GetAsync(url2);
            response.EnsureSuccessStatusCode();

            // Cevap içeriğini string olarak alın
            string responseBody2 = await response2.Content.ReadAsStringAsync();

            // JSON formatında ayrıştırma
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var issues2 = JsonSerializer.Deserialize<IdefixBookResult>(responseBody2, options2);

            // Sonuçları ekrana yazdırın
            foreach (var item in issues2.PageProps.CategoryData.Items)
            {
                string kitapAdi = "";
                string[] parts = item.Name.Split(new string[] { " - " }, StringSplitOptions.None);
                if (parts[0] == "İmzalı")
                    kitapAdi = parts[1];
                else
                    kitapAdi = parts[0];

                Console.WriteLine($"Yazar: {item.AuthorName}, Yayınevi: {item.BrandName}, Kitap Adı: {kitapAdi}\n");
            }
        }
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine("\nException Caught!");
        Console.WriteLine("Message :{0} ", e.Message);
    }
}

Console.ReadLine();
