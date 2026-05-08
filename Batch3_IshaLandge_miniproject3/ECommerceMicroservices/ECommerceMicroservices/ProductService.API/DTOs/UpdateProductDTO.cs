using System.ComponentModel.DataAnnotations;

public class UpdateProductDTO
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(500)]
    public string? Description { get; set; }
    [Required, Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    [Required, Range(0, int.MaxValue)]
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
}