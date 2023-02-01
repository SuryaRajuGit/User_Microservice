using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class UpdateUserDTO
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
            ErrorMessage = "Passwords must be at least 8 characters, one upper case (A-Z)," +
            "one lower case (a-z),a number (0-9) and special character (e.g. !@#$%^&*)")]
        public string? Password { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string? FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string? LastName { get; set; }

        [JsonProperty(PropertyName = "email_address")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Enter Valid email address")]
        public string? EmailAddress { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public PhoneDTO? Phone { get; set; }

        [JsonProperty(PropertyName = "address")]
        public AddressDTO? Address { get; set; }


    }
}
