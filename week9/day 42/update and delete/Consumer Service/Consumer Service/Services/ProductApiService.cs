using Consumer_Service.Models;
using System.Text.Json;

namespace Consumer_Service.Services
{
    public class ProductApiService: IProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProducts()
        {
            var response = await _httpClient.GetAsync($"https://localhost:7200/api/Products");

            var jsonData = await response.Content.ReadAsStringAsync();

            // It converts JSON strings into C# objects
            // Use JsonSerializer.Deserialize<T>(jsonString) for direct conversion

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var productData = JsonSerializer.Deserialize<List<Product>>(jsonData, options);

            return productData;
        }
        public async Task<Product> GetProductId(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7200/api/Products/{id}");


            var jsonData = await response.Content.ReadAsStringAsync();


            // It converts JSON strings into C# objects
            // Use JsonSerializer.Deserialize<T>(jsonString) for direct conversion

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var productData = await response.Content.ReadFromJsonAsync<Product>(options);

            return productData;
        }
        public async Task<Product> AddProduct(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7200/api/Products", product);

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            // returns result as json format
            var jsonData = await response.Content.ReadFromJsonAsync<Product>(options);

            return jsonData;
        }
        public async Task<Product> UpdateProduct(int id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", product);
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task<string> DeleteProduct(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
