using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class UpdateAddressDTO
    {
        ///<summary>
        /// address line 1
        ///</summary>
        [JsonProperty(PropertyName = "line1")]
        public string? Line1 { get; set; }

        ///<summary>
        /// address line 2
        ///</summary>
        [JsonProperty(PropertyName = "line2")]
        public string? Line2 { get; set; }

        ///<summary>
        /// city of the user
        ///</summary>
        [JsonProperty(PropertyName = "city")]
        public string? City { get; set; }

        ///<summary>
        /// Zipcode of the city
        ///</summary>
        [JsonProperty(PropertyName = "zipcode")]
        public string? Zipcode { get; set; }

        ///<summary>
        /// state name of the user
        ///</summary>
        [JsonProperty(PropertyName = "state_name")]
        public string? StateName { get; set; }

        ///<summary>
        /// Country of the user
        ///</summary>
        [JsonProperty(PropertyName = "country")]
        public string? Country { get; set; }

        ///<summary>
        /// Type of the user
        ///</summary>
        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; }
    }
}
