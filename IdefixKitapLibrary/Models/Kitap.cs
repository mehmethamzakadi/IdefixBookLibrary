namespace IdefixKitapLibrary.Models
{
    public class Kitap
    {
        public int Id { get; set; }
        public int IdefixId { get; set; }
        public string KitapAdi { get; set; }
        public string YazarAdi { get; set; }
        public string YayinEvi { get; set; }
        public string BasimDili { get; set; }
        public decimal? Fiyat { get; set; }
        public int? Stok { get; set; }
        public string Kategori { get; set; }
        public string ImageUrl { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
