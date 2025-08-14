using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PAWCP2.Models.DTOs
{


    public class FoodItemDto
    {
        [JsonPropertyName("foodItemId")]
        public int FoodItemId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("isPerishable")]
        public bool? IsPerishable { get; set; }

        [JsonPropertyName("caloriesPerServing")]
        public int? CaloriesPerServing { get; set; }

        [JsonPropertyName("supplier")]
        public string? Supplier { get; set; }

        [JsonPropertyName("isActive")]
        public bool? IsActive { get; set; }

        [JsonPropertyName("pricerange")]
        public decimal PriceRange { get; set; } = 0;

        [JsonPropertyName("expirationDate")]
        public DateOnly? ExpirationDate { get; set; }
    }
}


