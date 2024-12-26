using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Shopping_Cart_Server.Models;
using System.Data;

namespace Shopping_Cart_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExisitngController : Controller
    {
        private readonly IConfiguration _configuration;
        public ExisitngController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpGet]
        [Route("GetAllProducts")]
        public Response GetAllProducts()
        {
            List<Products> listProducts = new List<Products>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Eshoppingcart").ToString());
            var da = new SqlDataAdapter("Select * from tblProducts;", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Products products = new Products();
                    products.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    products.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    products.ActualPrice = Convert.ToDecimal(dt.Rows[i]["ActualPrice"]);
                    products.DiscountedPrice = Convert.ToDecimal(dt.Rows[i]["DiscountedPrice"]);
                    listProducts.Add(products);
                }
                if (listProducts.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Data Found";
                    response.listProducts = listProducts;

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No data found";
                    response.listProducts = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listProducts = null;

            }
            return response;
        }
    }
}