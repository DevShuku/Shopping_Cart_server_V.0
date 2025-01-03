﻿namespace Shopping_Cart_Server.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<Products> listProducts { get; set; }
        public List<CartItemResponse> CartItems { get; internal set; }
    }
}
