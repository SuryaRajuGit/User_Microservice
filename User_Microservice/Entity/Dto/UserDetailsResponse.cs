using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using User_Microservice.Entity.Models;

namespace User_Microservice.Entity.Dto
{
    public class UserDetailsResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName ="email")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("address")]
        public AddressDTO Address { get; set; }

        [JsonPropertyName("phone")]
        public PhoneDTO Phone { get; set; }


    }
}
