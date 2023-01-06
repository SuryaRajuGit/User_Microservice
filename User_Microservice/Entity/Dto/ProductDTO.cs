using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class ProductDTO
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [Required]
        [JsonPropertyName("qunatity")]
        public int Quantity { get; set; }

        [Required]
        [JsonPropertyName("price")]
        public float Price { get; set; }

        [Required]
        [JsonPropertyName("visibility")]
        public bool Visibility { get; set; }
    }
}
