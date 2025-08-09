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
    }
}

