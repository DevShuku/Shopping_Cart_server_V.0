public interface IShoppingRepository
{
    Task<Response> GetAllProductsAsync();
    Task<Response> AddItemToCartAsync(CartItem cartItem);
    Task<Response> GetCartItemsAsync();
    Task<Response> UpdateCartItemAsync(int cartItemId, int quantity);
    Task<Response> RemoveCartItemAsync(int cartItemId);
    Task<Response> ApplyCouponAsync(string couponCode);
}
