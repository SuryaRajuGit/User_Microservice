using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class ErrorDTO
    {
        ///<summary>
        /// type of error
        ///</summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        ///<summary>
        /// Message of the error
        ///</summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        ///<summary>
        /// status code of the error
        ///</summary>
        [JsonProperty(PropertyName = "status_code")]
        public string  StatusCode { get; set; }
    }
}
