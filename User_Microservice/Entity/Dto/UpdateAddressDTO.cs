using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class UpdateAddressDTO
    {
       
        [JsonProperty(PropertyName = "line1")]
        public string? Line1 { get; set; }

        [JsonProperty(PropertyName = "line2")]
        public string? Line2 { get; set; }

        
        [JsonProperty(PropertyName = "city")]
        public string? City { get; set; }

       
        [JsonProperty(PropertyName = "zipcode")]
        public string? Zipcode { get; set; }

        
        [JsonProperty(PropertyName = "state_name")]
        public string? StateName { get; set; }

        
        [JsonProperty(PropertyName = "country")]
        public string? Country { get; set; }

        
        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; }
    }
}
