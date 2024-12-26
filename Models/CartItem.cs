namespace Shopping_Cart_Server.Models
{
    public class CartItem
    {
        public int Id { get; set; } // Cart Item ID
        public int ProductId { get; set; } // Product ID
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
