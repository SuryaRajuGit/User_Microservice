using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class LoginResponseDTO
    {
        [JsonProperty(PropertyName = "jwt")]
        public string Jwt { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
