public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Eshoppingcart");
    }

    public async Task<Response> GetAllProductsAsync()
    {
        var response = new Response();
        var listProducts = new List<Products>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM tblProducts", connection);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var product = new Products
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Image = reader.GetString(reader.GetOrdinal("Image")),
                                ActualPrice = reader.GetDecimal(reader.GetOrdinal("ActualPrice")),
                                DiscountedPrice = reader.GetDecimal(reader.GetOrdinal("DiscountedPrice"))
                            };
                            listProducts.Add(product);
                        }
                    }
                }
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
                response.StatusMessage = "No Data Found";
                response.listProducts = null;
            }
        }
        // logginf is as extension to this project can be done 
        catch (SqlException sqlEx)
        {
            response.StatusCode = 500;
            response.StatusMessage = "Database error occurred: " + sqlEx.Message;
            response.listProducts = null;
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.StatusMessage = "An error occurred: " + ex.Message;
            response.listProducts = null;
        }

        return response;
    }
}
