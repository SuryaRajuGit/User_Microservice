using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class UpdateUpiDTO
    {
        ///<summary>
        /// id of the upi
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        ///<summary>
        /// bank name
        ///</summary>
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        ///<summary>
        /// Upi name
        ///</summary>
        [RegularExpression(@"^[\w.-]+@[\w.-]+$")]
        [JsonProperty(PropertyName = "upi")]
        public string? Upi { get; set; }
    }
}
