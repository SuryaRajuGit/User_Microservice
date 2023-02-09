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
       
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }


        [JsonProperty(PropertyName = "card_no")]
        public string CardNo { get; set; }


        [JsonProperty(PropertyName = "expiry_date")]
        public string ExpiryDate { get; set; }
    }
}
