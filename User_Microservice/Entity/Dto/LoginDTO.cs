using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace User_Microservice.Entity.Dto
{
    public class LoginDTO
    {
        ///<summary>
        /// Email address of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName ="email_address")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Enter Valid email address")]
        public string EmailAddress { get; set; }

        ///<summary>
        /// Password of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
