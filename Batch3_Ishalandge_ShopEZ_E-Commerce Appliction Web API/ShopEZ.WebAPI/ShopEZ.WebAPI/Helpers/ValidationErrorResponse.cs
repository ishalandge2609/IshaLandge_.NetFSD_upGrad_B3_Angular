namespace ShopEZ.WebAPI.Helpers
{
    public class ValidationErrorResponse
    {
       
            public bool Success { get; set; } = false;
            public string Message { get; set; } = "Validation failed";
            public List<string> Errors { get; set; }
            public int StatusCode { get; set; } = 400;
        }
    }

