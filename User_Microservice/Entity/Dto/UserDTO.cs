using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace User_Microservice.Entity.Dto
{
    public class UserDTO
    {
        ///<summary>
        /// Password of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName ="password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
            ErrorMessage = "Passwords must be at least 8 characters, one upper case (A-Z)," +
            "one lower case (a-z),a number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        ///<summary>
        /// First name of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        ///<summary>
        /// last name of the user
        ///</summary>
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        ///<summary>
        /// Email address of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "email_address")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Enter Valid email address")]
        public string EmailAddress { get; set; }

        ///<summary>
        /// Phone number of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "phone")]
        public PhoneDTO Phone { get; set; }


        [Required]
        [JsonProperty(PropertyName = "address")]
        public AddressDTO Address { get; set; }

        //[JsonProperty(PropertyName = "user_secret")]
        //public UserSecretDTO UserSecret { get; set; }

    }
}
