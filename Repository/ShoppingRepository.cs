using Microsoft.Data.SqlClient;
using Shopping_Cart_Server.Models;

public class ShoppingRepository : IShoppingRepository
{
    private readonly string _connectionString;

    public ShoppingRepository(IConfiguration configuration)
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
    public async Task<Response> AddItemToCartAsync(CartItem cartItem)
    {
        var response = new Response();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "INSERT INTO tblCart (ProductId, Quantity, Price) VALUES (@ProductId, @Quantity, @Price)",
                    connection
                );
                command.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
                command.Parameters.AddWithValue("@Quantity", cartItem.Quantity);
                command.Parameters.AddWithValue("@Price", cartItem.Price);
              
                await connection.OpenAsync();
                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Item added to cart successfully.";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Failed to add item to cart.";
                }
            }
        }
        catch (SqlException sqlEx)
        {
            response.StatusCode = 500;
            response.StatusMessage = "Database error occurred: " + sqlEx.Message;
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.StatusMessage = "An error occurred: " + ex.Message;
        }

        return response;
    }

    public async Task<Response> GetCartItemsAsync()
    {
        var response = new Response();
        var cartResponse = new List<CartItemResponse>(); 

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // var query = "SELECT ProductId, SUM(Quantity) AS Quantity,Price FROM  tblCart GROUP BY ProductId, Price";
                var query = "SELECT c.ProductId, p.Name, p.Image, SUM(c.Quantity) AS Quantity, c.Price FROM tblCart c " +
                            " JOIN tblProducts p ON c.ProductId = p.ID " +
                            " GROUP BY c.ProductId,p.Name, p.Image, c.Price";
                var command = new SqlCommand(query, connection);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        cartResponse.Add(new CartItemResponse
                        {
                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            ProductName = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image"))
                        });
                    }
                }

                response.StatusCode = 200;
                response.StatusMessage = "Cart items retrieved successfully.";
                response.CartItems = cartResponse;
            }
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.StatusMessage = "An error occurred: " + ex.Message;
        }

        return response;
    }

    public async Task<Response> RemoveCartItemAsync(int productId)
    {
        var response = new Response();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM tblCart WHERE ProductId = @ProductId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", productId);

                await connection.OpenAsync();
                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Cart item removed successfully.";
                }
                else
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "Cart item not found.";
                }
            }
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.StatusMessage = "An error occurred: " + ex.Message;
        }

        return response;
    }
}
