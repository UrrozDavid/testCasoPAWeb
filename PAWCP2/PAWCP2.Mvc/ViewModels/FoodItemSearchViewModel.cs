using PAWCP2.Models.DTOs;

namespace PAWCP2.Mvc.ViewModels;
public class FoodItemSearchViewModel
{
    public IEnumerable<FoodItemDto> FoodItems { get; set; } = Enumerable.Empty<FoodItemDto>();
    public List<string> Categories { get; set; } = new List<string>();
    public List<string> Brands { get; set; } = new List<string>();
    public List<string> Suppliers { get; set; } = new List<string>();

    // Filter properties
    public string? Category { get; set; }
    public string? Brand { get; set; }
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public bool? IsPerishable { get; set; }
    public int? CaloriesMax { get; set; }
    public string? Supplier { get; set; }
    public bool? IsActive { get; set; }

}