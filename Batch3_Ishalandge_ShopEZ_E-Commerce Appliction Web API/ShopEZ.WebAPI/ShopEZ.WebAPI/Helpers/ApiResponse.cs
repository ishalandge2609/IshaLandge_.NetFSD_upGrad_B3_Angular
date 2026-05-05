using System.Text.Json.Serialization;

namespace ShopEZ.WebAPI.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; }

        public int StatusCode { get; set; }
    }
}
