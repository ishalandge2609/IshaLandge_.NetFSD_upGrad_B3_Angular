namespace ShopEZ.WebAPI.DTOs
{
    public class ProductQueryDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public string? Search { get; set; } // Name search
        public string? Category { get; set; } // Filter

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
