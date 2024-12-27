using Shopping_Cart_Server.Models;

public interface IShoppingRepository
{
    Task<Response> GetAllProductsAsync();
    Task<Response> AddItemToCartAsync(CartItem cartItem);
    Task<Response> GetCartItemsAsync();
    Task<Response> RemoveCartItemAsync(int cartItemId);
}
