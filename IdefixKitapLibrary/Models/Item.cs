namespace IdefixKitapLibrary.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string BrandName { get; set; }
        public List<Image> Images { get; set; }
        public List<Property> Properties { get; set; }
        public List<Satici> Merchants { get; set; }
        public TimeStampInfo TimeStamp { get; set; }
        public string Name { get; set; }
    }
}
