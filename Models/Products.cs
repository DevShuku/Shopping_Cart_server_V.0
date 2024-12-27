namespace Shopping_Cart_Server.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal ActualPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
}
