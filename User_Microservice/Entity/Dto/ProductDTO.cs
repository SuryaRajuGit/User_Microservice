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
        ///<summary>
        /// Name of the product
        ///</summary>
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        ///<summary>
        /// Description of the product
        ///</summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        ///<summary>
        /// product quantity
        ///</summary>
        [Required]
        [JsonPropertyName("qunatity")]
        public int Quantity { get; set; }

        ///<summary>
        /// Product price
        ///</summary>
        [Required]
        [JsonPropertyName("price")]
        public float Price { get; set; }

        ///<summary>
        /// Visibility of the product
        ///</summary>
        [Required]
        [JsonPropertyName("visibility")]
        public bool Visibility { get; set; }
    }
}
