using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class CardDTO
    {
        ///<summary>
        /// Card id
        ///</summary>
        public Guid Id { get; set; }

        ///<summary>
        /// Name of the card
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        ///<summary>
        /// Card no
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "card_no")]
        public string CardNo { get; set; }

        ///<summary>
        /// Expiry date
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "expiry_date")]
        public string ExpiryDate { get; set; }
    }
}
