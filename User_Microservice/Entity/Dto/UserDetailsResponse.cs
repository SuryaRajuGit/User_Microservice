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
        ///<summary>
        /// id of the user
        ///</summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        ///<summary>
        /// first name of the user
        ///</summary>
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        ///<summary>
        /// last name of the user
        ///</summary>
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        ///<summary>
        /// email of the user
        ///</summary>
        [JsonProperty(PropertyName ="email")]
        public string EmailAddress { get; set; }

        ///<summary>
        /// Address of the user
        ///</summary>
        [JsonPropertyName("address")]
        public AddressDTO Address { get; set; }

        ///<summary>
        /// Phone of the user
        ///</summary>
        [JsonPropertyName("phone")]
        public PhoneDTO Phone { get; set; }


    }
}
