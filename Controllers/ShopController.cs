using Microsoft.AspNetCore.Mvc;
using Shopping_Cart_Server.Models;


namespace Shopping_Cart_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShoppingRepository _shoppingRepository;

        public ShopController(IShoppingRepository shoppingRepository)
        {
            _shoppingRepository = shoppingRepository;
        }
        #region GetAllProducts
        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var response = await _shoppingRepository.GetAllProductsAsync();

                if (response.StatusCode == 200)
                    return Ok(response);
                else if (response.StatusCode == 100)
                    return NotFound(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    StatusMessage = "An unexpected error occurred: " + ex.Message
                });
            }
        }
        #endregion

        #region AddItemToCart
        [HttpPost]
        [Route("AddItemToCart")]
        public async Task<IActionResult> AddItemToCartAsync([FromBody] CartItem cartItem)
        {
            try
            {
                if (cartItem == null || cartItem.ProductId <= 0 || cartItem.Quantity <= 0 || cartItem.Price <= 0)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        StatusMessage = "Invalid cart item details."
                    });
                }

                var response = await _shoppingRepository.AddItemToCartAsync(cartItem);

                if (response.StatusCode == 200)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    StatusMessage = "An unexpected error occurred: " + ex.Message
                });
            }
        }
        #endregion

        #region Cart actions
        [HttpGet]
        [Route("GetCartItems")]
        public async Task<IActionResult> GetCartItemsAsync()
        {
            try
            {
                var response = await _shoppingRepository.GetCartItemsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, StatusMessage = ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdateCartItem")]
        public async Task<IActionResult> UpdateCartItemAsync(int cartItemId, int quantity)
        {
            try
            {
                var response = await _shoppingRepository.UpdateCartItemAsync(cartItemId, quantity);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, StatusMessage = ex.Message });
            }
        }

        [HttpDelete]
        [Route("RemoveCartItem")]
        public async Task<IActionResult> RemoveCartItemAsync(int cartItemId)
        {
            try
            {
                var response = await _shoppingRepository.RemoveCartItemAsync(cartItemId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, StatusMessage = ex.Message });
            }
        }

        [HttpPost]
        [Route("ApplyCoupon")]
        public async Task<IActionResult> ApplyCouponAsync([FromBody] string couponCode)
        {
            try
            {
                var response = await _shoppingRepository.ApplyCouponAsync(couponCode);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, StatusMessage = ex.Message });
            }
        }
        #endregion
    }
}


