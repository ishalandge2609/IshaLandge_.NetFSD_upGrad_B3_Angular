using System.Net.Http.Headers;
using System.Net.Http.Json;
using OrderService.API.Exceptions;

namespace OrderService.API.Services
{
    public class ProductServiceClient
    {
        private readonly HttpClient _httpClient;

        public ProductServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductInfo?> GetProductAsync(int productId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"/api/products/{productId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductInfo>>();

            return result?.Data;
        }

        public async Task DeductStockAsync(int productId, int quantity, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync(
                $"/api/products/{productId}/stock",
                new { quantity }
            );

            if (!response.IsSuccessStatusCode)
                throw new ValidationException("Failed to update stock");
        }
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
    }

    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}