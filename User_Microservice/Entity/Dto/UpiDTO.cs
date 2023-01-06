using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class UpiDTO
    {
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "upi")]
        public string Upi { get; set; }
    }
}
