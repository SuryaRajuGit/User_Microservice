using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class UpiDTO
    {
        ///<summary>
        /// id of the upi
        ///</summary>
        public Guid Id { get; set; }

        ///<summary>
        /// bank name of the UPI
        ///</summary>
        [Required]  
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        ///<summary>
        /// upi address
        ///</summary>
        [Required]
        [RegularExpression(@"^[\w.-]+@[\w.-]+$")]
        [JsonProperty(PropertyName = "upi")]
        public string Upi { get; set; }
    }
}
