using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class AddressDTO
    {
        [Required]
        [JsonProperty(PropertyName = "line1")]
        public string Line1 { get; set; }

        [JsonProperty(PropertyName = "line2")]
        public string Line2 { get; set; }

        [Required]
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [Required]
        [JsonProperty(PropertyName = "zipcode")]
        public string Zipcode { get; set; }

        [Required]
        [JsonProperty(PropertyName = "state_name")]
        public string StateName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [Required]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
