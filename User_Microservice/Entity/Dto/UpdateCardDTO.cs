using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class UpdateCardDTO
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "user_id")]
        public Guid UserId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty(PropertyName = "card_no")]
        public string CardNo { get; set; }

        [Required]
        [JsonProperty(PropertyName = "expiry_date")]
        public string ExpiryDate { get; set; }
    }
}
