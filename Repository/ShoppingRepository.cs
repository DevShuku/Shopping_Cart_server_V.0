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
        var cartItems = new List<CartItem>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM tblCart";
                var command = new SqlCommand(query, connection);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cartItems.Add(new CartItem
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToDecimal(reader["Price"])
                        });
                    }
                }

                response.StatusCode = 200;
                response.StatusMessage = "Cart items retrieved successfully.";
                response.CartItems = cartItems;
            }
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.StatusMessage = "An error occurred: " + ex.Message;
        }

        return response;
    }

    public async Task<Response> UpdateCartItemAsync(int cartItemId, int quantity)
    {
        var response = new Response();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE tblCart SET Quantity = @Quantity WHERE Id = @CartItemId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@CartItemId", cartItemId);

                await connection.OpenAsync();
                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Cart item updated successfully.";
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

    public async Task<Response> RemoveCartItemAsync(int cartItemId)
    {
        var response = new Response();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM tblCart WHERE Id = @CartItemId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CartItemId", cartItemId);

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

    public async Task<Response> ApplyCouponAsync(string couponCode)
    {
        var response = new Response();

        try
        {
            if (couponCode == "DISCOUNT10")
            {
                response.StatusCode = 200;
                response.StatusMessage = "Coupon applied successfully. 10% discount applied.";
            }
            else
            {
                response.StatusCode = 400;
                response.StatusMessage = "Invalid coupon code.";
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
