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
        ///<summary>
        /// address line 1
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "line1")]
        public string Line1 { get; set; }

        ///<summary>
        /// address line 2
        ///</summary>
        [JsonProperty(PropertyName = "line2")]
        public string Line2 { get; set; }

        ///<summary>
        /// city of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        ///<summary>
        /// Zip code of the user address
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "zipcode")]
        public string Zipcode { get; set; }

        ///<summary>
        /// state name of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "state_name")]
        public string StateName { get; set; }

        ///<summary>
        /// Country of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        ///<summary>
        /// Address type
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
