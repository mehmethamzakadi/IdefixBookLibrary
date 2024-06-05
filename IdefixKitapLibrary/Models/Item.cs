namespace IdefixKitapLibrary.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string BrandName { get; set; }
        public List<Image> Images { get; set; }
        public List<Property> Properties { get; set; }
        public string Name { get; set; }
    }
}
