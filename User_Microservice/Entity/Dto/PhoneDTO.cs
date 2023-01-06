using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class PhoneDTO
    {
        [Required]
        [JsonProperty(PropertyName = "phone_number")]
        [Phone(ErrorMessage = "Enter valid phone number")]
        [MinLength(10,ErrorMessage ="Phone number must be of length 10")]
        public string PhoneNumber { get; set; }

        [Required]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
