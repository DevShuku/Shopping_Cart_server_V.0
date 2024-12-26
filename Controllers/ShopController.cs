namespace Shopping_Cart_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ShopController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var response = await _productRepository.GetAllProductsAsync();

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
    }
}


