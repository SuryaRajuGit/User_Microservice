using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class LoginResponseDTO
    {
        ///<summary>
        /// jwt token
        ///</summary>
        [JsonProperty(PropertyName = "jwt")]
        public string Jwt { get; set; }

        ///<summary>
        /// type of token
        ///</summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
