using IdefixKitapLibrary.Database;
using IdefixKitapLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Diagnostics;
using System.Text.Json;

namespace IdefixKitapLibrary.Islemler;

public static class Idefix
{
    public static async Task TumunuCek()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Console.WriteLine("Tümünü Çekme ve Kaydetme İşlemi Başladı..");

        using var context = new BookLibraryContext();
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(5);
        try
        {
            var pageIndex = 1;
            string kitapUrl = $"{ConfigurationManager.AppSettings["KitapUrl"]}";
            var endPoint = kitapUrl + pageIndex;

            HttpResponseMessage response = await client.GetAsync(endPoint);
            response.EnsureSuccessStatusCode(); // response içerisinde StatusCode 200 dışında dönerse hata fırlatır.
            string responseBody = await response.Content.ReadAsStringAsync();

            // JSON formatında ayrıştırma
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<IdefixBookResult>(responseBody, options);
            var totalRecordCount = result.PageProps.CategoryData.RecordCount;
            double pageSize = (totalRecordCount / 24);
            var totalPageSize = (int)Math.Round(pageSize, 0, MidpointRounding.AwayFromZero);

            for (pageIndex = 1; pageIndex < totalPageSize; pageIndex++)
            {
                var newEndPoint = $"{kitapUrl}{pageIndex}";
                response = await client.GetAsync(newEndPoint);

                while (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response = await Refresh(pageIndex, newEndPoint, client);
                }

                responseBody = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<IdefixBookResult>(responseBody, options);

                var bookList = new List<Kitap>();

                foreach (var item in result.PageProps.CategoryData.Items)
                {
                    string categoryName = item.Properties.FirstOrDefault(x => x.Text == "Kategori")?.ValueText;
                    string editionLanguage = item.Properties.FirstOrDefault(x => x.Text == "Basım Dili")?.ValueText;
                    var price = item.Merchants.Count > 0 ? item.Merchants[0].Price : null;
                    var stock = item.Merchants.Count > 0 ? item.Merchants[0]?.Stock : null;

                    if (!await context.Kitaplar.AnyAsync(x => x.IdefixId == item.Id))
                    {
                        var book = new Kitap
                        {
                            IdefixId = item.Id,
                            KitapAdi = item.Name,
                            BasimDili = editionLanguage,
                            Kategori = categoryName,
                            Fiyat = price,
                            Stok = stock,
                            CreatedDate = item.TimeStamp.CreatedAt.ToUniversalTime(),
                            UpdatedDate = item.TimeStamp.UpdatedAt.ToUniversalTime(),
                            YayinEvi = item.BrandName,
                            YazarAdi = item.AuthorName,
                            ImageUrl = item.Images.Count > 0 ? item.Images[0]?.Src : "",
                            ImageHeight = item.Images.Count > 0 ? item.Images[0].Height : 0,
                            ImageWidth = item.Images.Count > 0 ? item.Images[0].Height : 0,
                        };
                        bookList.Add(book);
                    }
                }

                if (bookList.Count > 0)
                {
                    await context.Kitaplar.AddRangeAsync(bookList);
                    await context.SaveChangesAsync();
                }

                var timeElapsed = stopwatch.Elapsed.ToString().Split('.')[0];
                var hours = timeElapsed.Split(':')[0];
                var minutes = timeElapsed.Split(':')[1];
                var seconds = timeElapsed.Split(':')[2];

                Console.WriteLine($"{pageIndex} Nolu Sayfa Eklendi. Kalan Sayfa: {totalPageSize - pageIndex} / Geçen Süre: {hours} Saat {minutes} Dakika {seconds} Saniye");
            }
            stopwatch.Stop();
            Console.WriteLine("Tümünü Çekme ve Kaydetme İşlemi Bitti..");
        }
        catch (HttpRequestException e)
        {
            stopwatch.Stop();
            Console.WriteLine("Hata :{0} ", e.Message);
        }
    }

    private static async Task<HttpResponseMessage> Refresh(int pageIndex, string url, HttpClient client)
    {
        await Task.Delay(10000); // 10 saniye bekler

        Console.WriteLine($"{pageIndex} Nolu Sayfa Yeniden istek atıldı.");

        HttpResponseMessage response = await client.GetAsync(url);

        return response;
    }

    public static async Task YeniEklenenKitaplariCek()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Console.WriteLine("Yeni Kitapları Çekme ve Kaydetme İşlemi Başladı..");

        using var context = new BookLibraryContext();
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(5);

        try
        {
            var pageIndex = 1;
            string kitapUrl = $"{ConfigurationManager.AppSettings["KitapUrl"]}";
            var endPoint = kitapUrl + pageIndex;

            HttpResponseMessage response = await client.GetAsync(endPoint);
            response.EnsureSuccessStatusCode(); // response içerisinde StatusCode 200 dışında dönerse hata fırlatır.
            string responseBody = await response.Content.ReadAsStringAsync();

            // JSON formatında ayrıştırma
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<IdefixBookResult>(responseBody, options);
            var totalRecordCount = result.PageProps.CategoryData.RecordCount;
            var currentRecorCount = context.Kitaplar.Count();
            var newAddedRecordCount = currentRecorCount > 0 ? totalRecordCount - currentRecorCount : totalRecordCount;

            var pageSize = Math.Ceiling(((decimal)newAddedRecordCount / 24));
            //var totalPageSize = (int)Math.Round(pageSize, 0, MidpointRounding.AwayFromZero);

            for (pageIndex = 1; pageIndex < pageSize; pageIndex++)
            {
                var newEndPoint = $"{kitapUrl}{pageIndex}";
                response = await client.GetAsync(newEndPoint);

                while (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response = await Refresh(pageIndex, newEndPoint, client);
                }

                responseBody = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<IdefixBookResult>(responseBody, options);

                var bookList = new List<Kitap>();

                foreach (var item in result.PageProps.CategoryData.Items)
                {
                    string categoryName = item.Properties.FirstOrDefault(x => x.Text == "Kategori")?.ValueText;
                    string editionLanguage = item.Properties.FirstOrDefault(x => x.Text == "Basım Dili")?.ValueText;
                    var price = item.Merchants.Count > 0 ? item.Merchants[0].Price : null;
                    var stock = item.Merchants.Count > 0 ? item.Merchants[0]?.Stock : null;

                    if (!await context.Kitaplar.AnyAsync(x => x.IdefixId == item.Id))
                    {
                        var book = new Kitap
                        {
                            IdefixId = item.Id,
                            KitapAdi = item.Name,
                            BasimDili = editionLanguage,
                            Kategori = categoryName,
                            Fiyat = price,
                            Stok = stock,
                            CreatedDate = item.TimeStamp.CreatedAt.ToUniversalTime(),
                            UpdatedDate = item.TimeStamp.UpdatedAt.ToUniversalTime(),
                            YayinEvi = item.BrandName,
                            YazarAdi = item.AuthorName,
                            ImageUrl = item.Images.Count > 0 ? item.Images[0]?.Src : "",
                            ImageHeight = item.Images.Count > 0 ? item.Images[0].Height : 0,
                            ImageWidth = item.Images.Count > 0 ? item.Images[0].Height : 0,
                        };
                        bookList.Add(book);
                    }
                }

                if (bookList.Count > 0)
                {
                    await context.Kitaplar.AddRangeAsync(bookList);
                    await context.SaveChangesAsync();
                }

                var timeElapsed = stopwatch.Elapsed.ToString().Split('.')[0];
                var hours = timeElapsed.Split(':')[0];
                var minutes = timeElapsed.Split(':')[1];
                var seconds = timeElapsed.Split(':')[2];

                Console.WriteLine($"{pageIndex} Nolu Sayfa Eklendi. Kalan Sayfa: {pageSize - pageIndex} / Geçen Süre: {hours} Saat {minutes} Dakika {seconds} Saniye");
            }

            Console.WriteLine($"Yeni Eklenen Kitap Sayısı: {newAddedRecordCount}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message} ");
        }
    }
}
